using Company.Framework.Messaging.Kafka.Producer.Context;
using Company.Framework.Messaging.Producer;

namespace Company.Framework.Messaging.Kafka.Producer;

public interface ITypedKafkaProducer : IProducer
{
    TypedKafkaProducerKey Key { get; }
}