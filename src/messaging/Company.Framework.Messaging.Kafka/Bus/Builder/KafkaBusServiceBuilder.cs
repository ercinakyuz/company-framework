using Company.Framework.Messaging.Bus.Builder;
using Company.Framework.Messaging.Consumer;
using Company.Framework.Messaging.Kafka.Consumer;
using Company.Framework.Messaging.Kafka.Producer;
using Company.Framework.Messaging.Kafka.Serialization;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Framework.Messaging.Kafka.Bus.Builder;

public class KafkaBusServiceBuilder: CoreBusServiceBuilder<KafkaBusBuilder>
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
                }).SetValueSerializer(serviceProvider.GetRequiredService<KafkaDefaultSerializer>()).Build();
                return new KafkaProducer($"{BusName}", producer);
            });
        return this;
    }

    public KafkaBusServiceBuilder WithConsumer<TConsumer, TMessage>(string name)
        where TConsumer : KafkaConsumer<TMessage>
    {
        ServiceCollection
            .AddSingleton<IConsumer, TConsumer>(serviceProvider =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var consumerGroupId = configuration.GetSection($"Messaging:{BusName}:Consumers:{name}:GroupId").Value;
                var defaultGroupId = configuration.GetSection($"Messaging:{BusName}:DefaultConsumerGroupId").Value;
                var consumer = new ConsumerBuilder<Null, TMessage>(new ConsumerConfig
                {
                    BootstrapServers = configuration.GetSection($"Messaging:{BusName}:Nodes").Value,
                    GroupId = consumerGroupId ?? defaultGroupId,
                    AllowAutoCreateTopics = true,
                }).SetValueDeserializer(serviceProvider.GetRequiredService<KafkaMessageDeserializer<TMessage>>()).Build();
                return ActivatorUtilities.CreateInstance<TConsumer>(serviceProvider, consumer, new KafkaConsumerSettings
                {
                    Topic = configuration.GetSection($"Messaging:{BusName}:Consumers:{name}:Topic").Value
                });
            });
        return this;
    }
}