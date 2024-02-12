using Company.Framework.Core.Id.Abstractions;
using Company.Framework.Messaging.Bus.Builder;
using Company.Framework.Messaging.Consumer;
using Company.Framework.Messaging.Consumer.Retrying;
using Company.Framework.Messaging.Kafka.AdminClient.Context;
using Company.Framework.Messaging.Kafka.AdminClient.Context.Provider;
using Company.Framework.Messaging.Kafka.Consumer;
using Company.Framework.Messaging.Kafka.Consumer.Context;
using Company.Framework.Messaging.Kafka.Consumer.Retrying.Handler;
using Company.Framework.Messaging.Kafka.Consumer.Settings;
using Company.Framework.Messaging.Kafka.Producer;
using Company.Framework.Messaging.Kafka.Producer.Context.Provider;
using Company.Framework.Messaging.Kafka.Producer.Settings;
using Company.Framework.Messaging.Kafka.Serialization;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static Company.Framework.Messaging.Constant.MessagingConstants;

namespace Company.Framework.Messaging.Kafka.Bus.Builder;

public class KafkaBusServiceBuilder : CoreBusServiceBuilder<KafkaBusBuilder>
{
    private const string KafkaPrefix = "Messaging:Kafka";
    private const string BusPrefix = $"{KafkaPrefix}:Buses";

    private readonly string _namedBusPrefix;

    public KafkaBusServiceBuilder(KafkaBusBuilder busBuilder, string busName) : base(busBuilder, busName)
    {
        _namedBusPrefix = $"{BusPrefix}:{BusName}";
    }
    internal KafkaBusServiceBuilder WithDefaultProducer()
    {
        return WithProducer(DefaultProducerName);
    }
    internal KafkaBusServiceBuilder WithAdminClientContext()
    {
        ServiceCollection.AddSingleton<IKafkaAdminClientContext>(serviceProvider =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var adminClient = new AdminClientBuilder(new AdminClientConfig
            {
                BootstrapServers = NodesFromConfiguration(configuration)
            }).Build();
            return new KafkaAdminClientContext(BusName, adminClient);
        });
        return this;
    }
    public KafkaBusServiceBuilder WithProducer(string name)
    {
        ServiceCollection
            .AddSingleton<IKafkaProducer, KafkaProducer>(serviceProvider =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var producer = new ProducerBuilder<Null, object>(new ProducerConfig
                {
                    BootstrapServers = NodesFromConfiguration(configuration),
                }).SetValueSerializer(serviceProvider.GetRequiredService<KafkaMessageSerializer<object>>()).Build();
                return new KafkaProducer(new KafkaProducerSettings(name, BusName), producer);
            });
        return this;
    }
    public KafkaBusServiceBuilder WithProducer<TId, TMessage>(string name) where TId : IId<TId> where TMessage : notnull
    {
        ServiceCollection.AddSingleton<ITypedKafkaProducer>(serviceProvider =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var producer = new ProducerBuilder<TId, TMessage>(new ProducerConfig
            {
                BootstrapServers = NodesFromConfiguration(configuration)
            }).SetKeySerializer(serviceProvider.GetRequiredService<KafkaIdSerializer<TId>>())
              .SetValueSerializer(serviceProvider.GetRequiredService<KafkaMessageSerializer<TMessage>>())
              .Build();
            var topic = configuration.GetSection($"{_namedBusPrefix}:Producers:{name}:Topic").Value;
            return new KafkaProducer<TId, TMessage>(new TypedKafkaProducerSettings(name, BusName, topic), producer);
        });
        return this;
    }

    public KafkaBusServiceBuilder WithConsumer<TConsumer, TMessage>(string name, ConsumerRetriability? retriability = default)
        where TConsumer : CoreKafkaConsumer<TMessage>
    {
        return WithConsumerInternal<TConsumer, Null, TMessage>(name, BuildConsumer<TMessage>, retriability);
    }

    public KafkaBusServiceBuilder WithConsumer<TConsumer, TId, TMessage>(string name, ConsumerRetriability? retriability = default)
    where TConsumer : CoreKafkaConsumer<TId, TMessage>
        where TId : IId<TId>
    {
        return WithConsumerInternal<TConsumer, TId, TMessage>(name, BuildConsumer<TId, TMessage>, retriability);
    }

    public KafkaBusServiceBuilder ThatConsume<TMessage>(string name, ConsumerRetriability? retriability = default) where TMessage : INotification
    {
        return ThatConsumeInternal(name, BuildConsumer<TMessage>, retriability);
    }

    public KafkaBusServiceBuilder ThatConsume<TId, TMessage>(string name, ConsumerRetriability? retriability = default)
        where TId : IId<TId>
        where TMessage : INotification
    {
        return ThatConsumeInternal(name, BuildConsumer<TId, TMessage>, retriability);
    }

    public KafkaBusServiceBuilder ThatConsumeInternal<TId, TMessage>(
        string name,
        Func<IServiceProvider, IConfiguration, IConfigurationSection, IConsumer<TId, TMessage>> consumerFactory,
        ConsumerRetriability? retriability = default)
        where TMessage : INotification
    {
        return WithConsumerInternal<DefaultKafkaConsumer<TId, TMessage>, TId, TMessage>(name, consumerFactory, retriability);
    }

    private KafkaBusServiceBuilder WithConsumerInternal<TConsumer, TId, TMessage>(
         string name,
         Func<IServiceProvider, IConfiguration, IConfigurationSection, IConsumer<TId, TMessage>> consumerFactory,
         ConsumerRetriability? retriability = default)
         where TConsumer : CoreKafkaConsumer<TId, TMessage>
    {
        ServiceCollection
            .AddSingleton<IConsumer, TConsumer>(serviceProvider =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var consumerSection = configuration.GetSection($"{_namedBusPrefix}:Consumers:{name}");
                var consumer = consumerFactory(serviceProvider, configuration, consumerSection);
                var topic = consumerSection.GetSection("Topic").Value;
                var settings = new KafkaConsumerSettings(name, topic);
                var retryingHandler = BuildRetryingHandler<TId, TMessage>(retriability, serviceProvider, consumerSection, topic);
                var adminClientContext = serviceProvider.GetRequiredService<IKafkaAdminClientContextProvider>().Resolve(BusName);
                var context = new KafkaConsumerContext<TId, TMessage>(consumer, settings, adminClientContext, retryingHandler);
                return ActivatorUtilities.CreateInstance<TConsumer>(serviceProvider, context);
            });
        return this;
    }

    private IKafkaConsumerRetryingHandler? BuildRetryingHandler(ConsumerRetriability? retriability, IServiceProvider serviceProvider, IConfigurationSection consumerSection, string topic)
    {
        if (retriability == default)
            return default;

        var kafkaRetrySettings = consumerSection.GetSection("Retry").Get<KafkaRetrySettings>();
        kafkaRetrySettings.Topic.Name = $"retry_{topic}";
        var producer = serviceProvider.GetRequiredService<IKafkaProducerContextProvider>().Resolve(BusName).Default();
        return ActivatorUtilities.CreateInstance<KafkaConsumerRetryingHandler>(serviceProvider, producer, retriability, kafkaRetrySettings);
    }

    private IKafkaConsumerRetryingHandler<TId, TMessage>? BuildRetryingHandler<TId, TMessage>(ConsumerRetriability? retriability, IServiceProvider serviceProvider, IConfigurationSection consumerSection, string topic)
    {
        if (retriability == default)
            return default;

        var kafkaRetrySettings = consumerSection.GetSection("Retry").Get<KafkaRetrySettings>();
        kafkaRetrySettings.Topic.Name = $"retry_{topic}";
        var producer = serviceProvider.GetRequiredService<ITypedKafkaProducerContextProvider>().Resolve(BusName)!.Resolve<TId, TMessage>();
        return ActivatorUtilities.CreateInstance<KafkaConsumerRetryingHandler<TId, TMessage>>(serviceProvider, producer, retriability, kafkaRetrySettings);
    }

    private IConsumer<Null, TMessage> BuildConsumer<TMessage>(IServiceProvider serviceProvider, IConfiguration configuration, IConfigurationSection consumerSection)
    {
        return ConsumerBuilder<Null, TMessage>(configuration, consumerSection)
        .SetValueDeserializer(serviceProvider.GetRequiredService<KafkaMessageDeserializer<TMessage>>())
        .Build();
    }

    private IConsumer<TId, TMessage> BuildConsumer<TId, TMessage>(IServiceProvider serviceProvider, IConfiguration configuration, IConfigurationSection consumerSection)
        where TId : IId<TId>
    {
        return ConsumerBuilder<TId, TMessage>(configuration, consumerSection)
        .SetKeyDeserializer(serviceProvider.GetRequiredService<KafkaIdDeserializer<TId>>())
        .SetValueDeserializer(serviceProvider.GetRequiredService<KafkaMessageDeserializer<TMessage>>())
        .Build();
    }

    private ConsumerBuilder<TId, TMessage> ConsumerBuilder<TId, TMessage>(IConfiguration configuration, IConfigurationSection consumerSection)
    {
        return new ConsumerBuilder<TId, TMessage>(new ConsumerConfig
        {
            BootstrapServers = NodesFromConfiguration(configuration),
            GroupId = BuildGroupId(configuration, consumerSection),
            AutoOffsetReset = AutoOffsetReset.Latest
        });
    }

    private string? BuildGroupId(IConfiguration configuration, IConfigurationSection consumerSection)
    {
        return consumerSection.GetSection("GroupId").Value ?? configuration.GetSection($"{KafkaPrefix}:Defaults:ConsumerGroupId").Value;
    }

    private string NodesFromConfiguration(IConfiguration configuration)
    {
        return configuration.GetSection($"{_namedBusPrefix}:Nodes").Value;
    }



}