using Company.Framework.Messaging.Consumer.Settings;

namespace Company.Framework.Messaging.Kafka.Consumer.Settings;

public class KafkaTopicSettings : CoreRetrySettings
{
    public string Name { get; set; }

    public short Replication { get; set; }

    public short Partition { get; set; }
}