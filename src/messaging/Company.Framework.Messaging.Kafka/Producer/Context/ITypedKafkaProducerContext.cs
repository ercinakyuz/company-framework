using Company.Framework.Messaging.Producer.Context;

namespace Company.Framework.Messaging.Kafka.Producer.Context;

public interface ITypedKafkaProducerContext : IProducerContext
{
    IKafkaProducer<TId, TMessage> Resolve<TId, TMessage>();
}