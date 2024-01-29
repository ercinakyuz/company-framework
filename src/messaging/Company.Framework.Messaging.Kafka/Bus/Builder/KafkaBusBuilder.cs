using Company.Framework.Messaging.Bus;
using Company.Framework.Messaging.Bus.Builder;
using Company.Framework.Messaging.Kafka.AdminClient.Context.Provider;
using Company.Framework.Messaging.Kafka.Producer.Context.Provider;
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
        ServiceCollection.AddSingleton<IBus>(serviceProvider => ActivatorUtilities.CreateInstance<KafkaBus>(serviceProvider, busName));
        return new KafkaBusServiceBuilder(this, busName).WithAdminClientContext().WithDefaultProducer();
    }
    internal KafkaBusBuilder WithDefaultSerialization()
    {
        ServiceCollection
            .AddSingleton(typeof(KafkaIdSerializer<>))
            .AddSingleton(typeof(KafkaMessageSerializer<>))
            .AddSingleton(typeof(KafkaIdDeserializer<>))
            .AddSingleton(typeof(KafkaMessageDeserializer<>));
        return this;
    }
    internal KafkaBusBuilder WithProviders()
    {
        ServiceCollection
            .AddSingleton<IKafkaAdminClientContextProvider, KafkaAdminClientContextProvider>()
            .AddSingleton<IKafkaProducerContextProvider, KafkaProducerContextProvider>()
            .AddSingleton<ITypedKafkaProducerContextProvider, TypedKafkaProducerContextProvider>();
        return this;
    }

    public MainBusServiceBuilder BuildKafka()
    {
        return Build();
    }
}