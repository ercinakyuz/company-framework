using Company.Framework.Messaging.Consumer.Retrying;
using Company.Framework.Messaging.Kafka.Consumer.Settings;

namespace Company.Framework.Messaging.Kafka.Consumer.Retrying.Handler;

public interface IKafkaConsumerRetryingHandler : IConsumerRetryingHandler
{
    KafkaTopicSettings TopicSettings { get; }
}