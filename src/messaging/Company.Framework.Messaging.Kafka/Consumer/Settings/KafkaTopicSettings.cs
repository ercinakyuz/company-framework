namespace Company.Framework.Messaging.Kafka.Consumer.Settings;

public class KafkaTopicSettings
{
    public required string Name { get; set; }

    public required short Replication { get; init; }

    public required short Partition { get; init; }
}