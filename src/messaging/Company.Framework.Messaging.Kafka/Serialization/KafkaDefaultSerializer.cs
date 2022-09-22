using System.Text.Json;
using Confluent.Kafka;

namespace Company.Framework.Messaging.Kafka.Serialization;

public class KafkaDefaultSerializer : ISerializer<object>
{
    private readonly KafkaSerializationSettings _kafkaSerializationSettings;

    public KafkaDefaultSerializer(KafkaSerializationSettings kafkaSerializationSettings)
    {
        _kafkaSerializationSettings = kafkaSerializationSettings;
    }
    public byte[] Serialize(object data, SerializationContext context)
    {
        return JsonSerializer.SerializeToUtf8Bytes(data, _kafkaSerializationSettings.JsonSerializerOptions);
    }
}