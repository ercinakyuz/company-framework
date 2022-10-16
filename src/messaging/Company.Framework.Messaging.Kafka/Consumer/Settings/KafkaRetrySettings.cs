using Company.Framework.Messaging.Consumer.Settings;

namespace Company.Framework.Messaging.Kafka.Consumer.Settings;

public class KafkaRetrySettings : CoreRetrySettings
{
    public KafkaTopicSettings Topic { get; set; }
}