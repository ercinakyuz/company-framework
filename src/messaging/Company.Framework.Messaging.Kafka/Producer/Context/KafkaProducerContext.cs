using Company.Framework.Messaging.Bus;
using Company.Framework.Messaging.Producer.Context;

namespace Company.Framework.Messaging.Kafka.Producer.Context
{
    public class KafkaProducerContext : CoreProducerContext<IKafkaProducer>, IKafkaProducerContext
    {
        public KafkaProducerContext(IEnumerable<IKafkaProducer> producers) : base(producers, BusType.Kafka)
        {
        }
    }
}
