using Company.Framework.Core.Id.Abstractions;
using Company.Framework.Messaging.Consumer.Retrying;
using Company.Framework.Messaging.Consumer.Retrying.Args;
using Company.Framework.Messaging.Kafka.Consumer.Settings;

namespace Company.Framework.Messaging.Kafka.Consumer.Retrying.Handler;

public interface IKafkaConsumerRetryingHandler : IConsumerRetryingHandler
{
    KafkaTopicSettings TopicSettings { get; }
}

public interface IKafkaConsumerRetryingHandler<TId, TMessage> : IConsumerRetryingHandler<KafkaConsumerRetrialArgs<TId, TMessage>>
{
    KafkaTopicSettings TopicSettings { get; }
}