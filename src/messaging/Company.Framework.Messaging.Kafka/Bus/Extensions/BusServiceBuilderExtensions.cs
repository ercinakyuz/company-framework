using Company.Framework.Messaging.Bus.Builder;
using Company.Framework.Messaging.Kafka.Bus.Builder;

namespace Company.Framework.Messaging.Kafka.Bus.Extensions
{
    public static class MainBusServiceBuilderExtensions
    {
        public static KafkaBusBuilder WithKafka(this MainBusServiceBuilder mainBusServiceBuilder)
        {
            var kafkaServiceBuilder = new KafkaBusBuilder(mainBusServiceBuilder).WithDefaultSerialization().WithProviders();
            return kafkaServiceBuilder;
        }
    }
}
