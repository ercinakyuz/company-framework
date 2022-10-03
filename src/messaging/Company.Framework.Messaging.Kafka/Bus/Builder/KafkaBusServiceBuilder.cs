using Company.Framework.Messaging.Bus.Builder;
using Company.Framework.Messaging.Consumer;
using Company.Framework.Messaging.Kafka.Consumer;
using Company.Framework.Messaging.Kafka.Consumer.Context;
using Company.Framework.Messaging.Kafka.Producer;
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
                }).SetValueSerializer(serviceProvider.GetRequiredService<KafkaDefaultSerializer>()).Build();
                return new KafkaProducer($"{BusName}", producer);
            });
        return this;
    }

    public KafkaBusServiceBuilder WithConsumer<TConsumer, TMessage>(string name)
        where TConsumer : AbstractKafkaConsumer<TMessage>
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
                var settings = new KafkaConsumerSettings
                {
                    Name = name,
                    Topic = configuration.GetSection($"Messaging:{BusName}:Consumers:{name}:Topic").Value
                };
                var context = new KafkaConsumerContext<TMessage>(consumer, settings);
                return ActivatorUtilities.CreateInstance<TConsumer>(serviceProvider, context);
            });
        return this;
    }

    public KafkaBusServiceBuilder ThatConsume<TMessage>(string name) where TMessage : INotification
    {
        return WithConsumer<GenericKafkaConsumer<TMessage>, TMessage>(name);
    }

}