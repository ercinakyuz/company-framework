namespace Company.Framework.Messaging.Kafka.Serialization;

public class KafkaDefaultSerializer : KafkaMessageSerializer<object>
{
    public KafkaDefaultSerializer(KafkaSerializationSettings kafkaSerializationSettings) : base(kafkaSerializationSettings)
    {
    }
}