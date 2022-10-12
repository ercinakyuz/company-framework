using System.Text.Json;
using Company.Framework.Core.Identity;
using Confluent.Kafka;

namespace Company.Framework.Messaging.Kafka.Serialization;

public class KafkaMessageSerializer<TMessage> : ISerializer<TMessage>
{
    private readonly KafkaSerializationSettings _kafkaSerializationSettings;

    public KafkaMessageSerializer(KafkaSerializationSettings kafkaSerializationSettings)
    {
        _kafkaSerializationSettings = kafkaSerializationSettings;
    }
    public byte[] Serialize(TMessage message, SerializationContext context)
    {
        return JsonSerializer.SerializeToUtf8Bytes(message, _kafkaSerializationSettings.JsonSerializerOptions);
    }
}

public class KafkaIdSerializer<TId> : ISerializer<TId> where TId: CoreId<TId>
{
    private readonly KafkaSerializationSettings _kafkaSerializationSettings;

    public KafkaIdSerializer(KafkaSerializationSettings kafkaSerializationSettings)
    {
        _kafkaSerializationSettings = kafkaSerializationSettings;
    }
    public byte[] Serialize(TId id, SerializationContext context)
    {
        return JsonSerializer.SerializeToUtf8Bytes(id.ToString(), _kafkaSerializationSettings.JsonSerializerOptions);
    }
}