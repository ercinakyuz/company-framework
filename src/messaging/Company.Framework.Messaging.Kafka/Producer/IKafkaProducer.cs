using Company.Framework.Core.Id.Abstractions;
using Company.Framework.Messaging.Kafka.Producer.Args;
using Company.Framework.Messaging.Producer;

namespace Company.Framework.Messaging.Kafka.Producer;

public interface IKafkaProducer : IProducer<KafkaProduceArgs>;

public interface IKafkaProducer<TId, TMessage> : ITypedKafkaProducer, IProducer<KafkaProduceArgs<TId, TMessage>>;
