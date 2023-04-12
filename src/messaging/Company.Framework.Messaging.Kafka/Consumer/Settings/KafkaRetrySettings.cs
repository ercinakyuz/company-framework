using Company.Framework.Messaging.Consumer.Settings;

namespace Company.Framework.Messaging.Kafka.Consumer.Settings;

public class KafkaRetrySettings : CoreRetrySettings
{
    public required KafkaTopicSettings Topic { get; init; }
}