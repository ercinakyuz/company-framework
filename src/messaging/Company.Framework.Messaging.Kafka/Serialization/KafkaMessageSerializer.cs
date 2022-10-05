using System.Text.Json;
using Confluent.Kafka;

namespace Company.Framework.Messaging.Kafka.Serialization;

public class KafkaMessageSerializer<TMessage> : ISerializer<TMessage>
{
    private readonly KafkaSerializationSettings _kafkaSerializationSettings;

    public KafkaMessageSerializer(KafkaSerializationSettings kafkaSerializationSettings)
    {
        _kafkaSerializationSettings = kafkaSerializationSettings;
    }
    public byte[] Serialize(TMessage data, SerializationContext context)
    {
        return JsonSerializer.SerializeToUtf8Bytes(data, _kafkaSerializationSettings.JsonSerializerOptions);
    }
}