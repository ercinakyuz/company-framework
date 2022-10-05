using System.Text.Json;
using Company.Framework.Messaging.Bus.Builder;
using Company.Framework.Messaging.Kafka.Producer.Context;
using Company.Framework.Messaging.Kafka.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Framework.Messaging.Kafka.Bus.Builder;

public class KafkaBusBuilder : CoreBusBuilder<KafkaBusBuilder>
{
    public KafkaBusBuilder(MainBusServiceBuilder mainBusServiceBuilder) : base(mainBusServiceBuilder)
    {
    }
    public KafkaBusServiceBuilder WithBus(string busName)
    {
        return new KafkaBusServiceBuilder(this, busName).WithDefaultProducer();
    }
    internal KafkaBusBuilder WithDefaultSerialization()
    {
        ServiceCollection
            .AddSingleton(typeof(KafkaMessageSerializer<>))
            .AddSingleton(typeof(KafkaMessageDeserializer<>))
            .AddSingleton(_ => new KafkaSerializationSettings(new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }));
        return this;
    }

    internal KafkaBusBuilder WithProducerContext()
    {
        return WithProducerContext<IKafkaProducerContext, KafkaProducerContext>();
    }

    public MainBusServiceBuilder BuildKafka()
    {
        return Build();
    }
}