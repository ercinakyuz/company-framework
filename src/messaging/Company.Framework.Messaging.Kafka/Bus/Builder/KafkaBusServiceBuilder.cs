using Company.Framework.Core.Delay;
using Company.Framework.Messaging.Bus.Builder;
using Company.Framework.Messaging.Consumer;
using Company.Framework.Messaging.Consumer.Retrial;
using Company.Framework.Messaging.Consumer.Settings;
using Company.Framework.Messaging.Kafka.Consumer;
using Company.Framework.Messaging.Kafka.Consumer.Context;
using Company.Framework.Messaging.Kafka.Consumer.Retrial;
using Company.Framework.Messaging.Kafka.Consumer.Retrial.Context;
using Company.Framework.Messaging.Kafka.Consumer.Settings;
using Company.Framework.Messaging.Kafka.Producer;
using Company.Framework.Messaging.Kafka.Producer.Context;
using Company.Framework.Messaging.Kafka.Serialization;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Framework.Messaging.Kafka.Bus.Builder;

public class KafkaBusServiceBuilder : CoreBusServiceBuilder<KafkaBusBuilder>
{
    public KafkaBusServiceBuilder(KafkaBusBuilder busBuilder, string busName) : base(busBuilder, busName)
    {
    }
    internal KafkaBusServiceBuilder WithDefaultProducer()
    {
        ServiceCollection
            .AddSingleton<IKafkaProducer, KafkaProducer>(serviceProvider =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var producer = new ProducerBuilder<Null, object>(new ProducerConfig
                {
                    BootstrapServers = configuration.GetSection($"Messaging:{BusName}:Nodes").Value,
                }).SetValueSerializer(serviceProvider.GetRequiredService<KafkaMessageSerializer<object>>()).Build();
                return new KafkaProducer($"{BusName}", producer);
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
                var consumerGroupId = configuration.GetSection($"Messaging:{BusName}:Consumers:{name}:GroupId").Value;
                var defaultGroupId = configuration.GetSection($"Messaging:{BusName}:DefaultConsumerGroupId").Value;
                var nodes = configuration.GetSection($"Messaging:{BusName}:Nodes").Value;
                var consumer = new ConsumerBuilder<Ignore, TMessage>(new ConsumerConfig
                {
                    BootstrapServers = nodes,
                    GroupId = consumerGroupId ?? defaultGroupId,
                    AllowAutoCreateTopics = true,
                }).SetValueDeserializer(serviceProvider.GetRequiredService<KafkaMessageDeserializer<TMessage>>()).Build();
                var topic = configuration.GetSection($"Messaging:{BusName}:Consumers:{name}:Topic").Value;
                var settings = new KafkaConsumerSettings(name, topic);
                IKafkaRetrialContext? retrialContext = default;
                if (retriability != default)
                {
                    Enum.TryParse<DelayType>(configuration.GetSection($"Messaging:{BusName}:Consumers:{name}:Retry:Delay:Type").Value, out var delayType);
                    var delayInterval = TimeSpan.FromMilliseconds(long.Parse(configuration.GetSection($"Messaging:{BusName}:Consumers:{name}:Retry:Delay:IntervalMs").Value));
                    var kafkaRetrySettings = new KafkaRetrySettings(
                        $"retry_{topic}",
                        short.Parse(configuration.GetSection($"Messaging:{BusName}:Consumers:{name}:Retry:Count").Value),
                        new DelaySettings(delayType, delayInterval)
                    );
                    var producer = serviceProvider.GetRequiredService<IKafkaProducerContext>().Resolve(BusName);
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

}