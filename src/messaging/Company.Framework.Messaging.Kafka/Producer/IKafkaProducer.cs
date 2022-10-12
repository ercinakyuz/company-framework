using Company.Framework.Messaging.Kafka.Producer.Args;
using Company.Framework.Messaging.Producer;

namespace Company.Framework.Messaging.Kafka.Producer;

public interface IKafkaProducer : IProducer
{
    Task ProduceAsync(KafkaProduceArgs args, CancellationToken cancellationToken);
}

public interface IKafkaProducer<TId, TMessage> : ITypedKafkaProducer
{
    Task ProduceAsync(KafkaProduceArgs<TId, TMessage> args, CancellationToken cancellationToken);
}