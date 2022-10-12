using Company.Framework.Messaging.Bus;
using Company.Framework.Messaging.Kafka.Producer.Context;

namespace Company.Framework.Messaging.Kafka.Bus;

public interface IKafkaBus : IBus
{
    public IKafkaProducerContext ProducerContext { get; }
    ITypedKafkaProducerContext TypedProducerContext { get; }
}