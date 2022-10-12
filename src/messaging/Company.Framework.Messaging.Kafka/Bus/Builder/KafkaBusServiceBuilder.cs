using Company.Framework.Core.Identity;
using Company.Framework.Messaging.Bus.Builder;
using Company.Framework.Messaging.Consumer;
using Company.Framework.Messaging.Consumer.Retrial;
using Company.Framework.Messaging.Kafka.Consumer;
using Company.Framework.Messaging.Kafka.Consumer.Context;
using Company.Framework.Messaging.Kafka.Consumer.Retrial.Context;
using Company.Framework.Messaging.Kafka.Consumer.Settings;
using Company.Framework.Messaging.Kafka.Producer;
using Company.Framework.Messaging.Kafka.Producer.Context;
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
    private const string BusPrefix = "Messaging:Kafka:Buses";

    private readonly string _namedBusPrefix;

    public KafkaBusServiceBuilder(KafkaBusBuilder busBuilder, string busName) : base(busBuilder, busName)
    {
        _namedBusPrefix = $"{BusPrefix}:{BusName}";
    }
    internal KafkaBusServiceBuilder WithDefaultProducer()
    {
        ServiceCollection
            .AddSingleton<IKafkaProducer, KafkaProducer>(serviceProvider =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var producer = new ProducerBuilder<Null, object>(new ProducerConfig
                {
                    BootstrapServers = NodesFromConfiguration(configuration),
                }).SetValueSerializer(serviceProvider.GetRequiredService<KafkaMessageSerializer<object>>()).Build();
                return new KafkaProducer(new KafkaProducerSettings(DefaultProducerName, BusName), producer);
            });
        return this;
    }

    public KafkaBusServiceBuilder WithProducer<TId, TMessage>(string name) where TId : CoreId<TId>
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
        ServiceCollection
            .AddSingleton<IConsumer, TConsumer>(serviceProvider =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var consumerSection = configuration.GetSection($"{_namedBusPrefix}:Consumers:{name}");
                var consumerGroupId = consumerSection.GetSection("GroupId").Value;
                var consumer = new ConsumerBuilder<Ignore, TMessage>(new ConsumerConfig
                {
                    BootstrapServers = NodesFromConfiguration(configuration),
                    GroupId = consumerGroupId ?? configuration.GetSection($"{BusPrefix}:Defaults:ConsumerGroupId").Value,
                    AllowAutoCreateTopics = true,
                }).SetValueDeserializer(serviceProvider.GetRequiredService<KafkaMessageDeserializer<TMessage>>()).Build();
                var topic = consumerSection.GetSection("Topic").Value;
                var settings = new KafkaConsumerSettings(name, topic);
                IKafkaRetrialContext? retrialContext = default;
                if (retriability != default)
                {
                    var kafkaRetrySettings = consumerSection.GetSection("Retry").Get<KafkaRetrySettings>();
                    kafkaRetrySettings.Topic = $"retry_{topic}";
                    var producer = serviceProvider.GetRequiredService<IKafkaProducerContextProvider>().Resolve(BusName).Default();
                    retrialContext = ActivatorUtilities.CreateInstance<KafkaRetrialContext>(serviceProvider, producer, retriability, kafkaRetrySettings);
                }
                var context = new KafkaConsumerContext<TMessage>(consumer, settings, retrialContext);
                return ActivatorUtilities.CreateInstance<TConsumer>(serviceProvider, context);
            });
        return this;
    }

    public KafkaBusServiceBuilder ThatConsume<TMessage>(string name, ConsumerRetriability? retryContext = default) where TMessage : INotification
    {
        return WithConsumer<DefaultKafkaConsumer<TMessage>, TMessage>(name, retryContext);
    }

    private string NodesFromConfiguration(IConfiguration configuration)
    {
        return configuration.GetSection($"{_namedBusPrefix}:Nodes").Value;
    }

}