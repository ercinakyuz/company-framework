using System.Text.Json;
using Company.Framework.Messaging.Consumer;
using Company.Framework.Messaging.Kafka.Consumer;
using Company.Framework.Messaging.Kafka.Producer;
using Company.Framework.Messaging.Kafka.Serialization;
using Company.Framework.Messaging.Producer;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Framework.Messaging.Kafka.Extensions
{
    public static class KafkaServiceCollectionExtensions
    {
        public static KafkaServicesBuilder AddKafka(this IServiceCollection serviceCollection)
        {
            var kafkaServiceBuilder = new KafkaServicesBuilder(serviceCollection).WithDefaultSerialization().WithDefaultProducer();
            return kafkaServiceBuilder;
        }

        public class KafkaServicesBuilder
        {
            private readonly IServiceCollection _serviceCollection;

            public KafkaServicesBuilder(IServiceCollection serviceCollection)
            {
                _serviceCollection = serviceCollection;
            }

            public IServiceCollection Build()
            {
                return _serviceCollection;
            }

            public KafkaServicesBuilder WithDefaultSerialization()
            {
                _serviceCollection
                    .AddSingleton<KafkaDefaultSerializer>()
                    .AddSingleton(typeof(KafkaMessageDeserializer<>))
                    .AddSingleton(_ => new KafkaSerializationSettings(new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    }));
                return this;
            }

            public KafkaServicesBuilder WithDefaultProducer()
            {
                _serviceCollection
                    .AddSingleton(serviceProvider =>
                    {
                        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                        return new ProducerBuilder<Null, object>(new ProducerConfig
                        {
                            BootstrapServers = configuration.GetSection("Messaging:Kafka:Nodes").Value,
                        }).SetValueSerializer(serviceProvider.GetRequiredService<KafkaDefaultSerializer>()).Build();
                    })
                    .AddSingleton<IProducer, KafkaProducer>();
                return this;
            }

            public KafkaServicesBuilder WithConsumer<TConsumer, TMessage>()
                where TConsumer : KafkaConsumer<TMessage>
            {
                _serviceCollection
                    .AddSingleton(serviceProvider =>
                    {
                        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                        return new ConsumerBuilder<Null, TMessage>(new ConsumerConfig
                        {
                            BootstrapServers = configuration.GetSection("Messaging:Kafka:Nodes").Value,
                            GroupId = configuration.GetSection("Messaging:Kafka:DefaultConsumerGroupId").Value,
                            AllowAutoCreateTopics = true,
                        }).SetValueDeserializer(serviceProvider.GetRequiredService<KafkaMessageDeserializer<TMessage>>()).Build();
                    })
                    .AddSingleton<IConsumer, TConsumer>();
                return this;
            }
        }
    }
}
