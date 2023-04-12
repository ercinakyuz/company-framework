using Company.Framework.Messaging.Consumer.Settings;

namespace Company.Framework.Messaging.Kafka.Consumer.Settings;

public class KafkaTopicSettings : CoreRetrySettings
{
    public required string Name { get; set; }

    public required short Replication { get; init; }

    public required short Partition { get; init; }
}