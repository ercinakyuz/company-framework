using System.Runtime.Serialization;
using System.Text.Json;
using Confluent.Kafka;

namespace Company.Framework.Messaging.Kafka.Serialization;

public class KafkaMessageDeserializer<TMessage> : IDeserializer<TMessage>
{
    private readonly KafkaSerializationSettings _kafkaSerializationSettings;

    public KafkaMessageDeserializer(KafkaSerializationSettings kafkaSerializationSettings)
    {
        _kafkaSerializationSettings = kafkaSerializationSettings;
    }
    public TMessage Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        if (isNull)
            throw new SerializationException("Byte array of message cannot be null");
        return JsonSerializer.Deserialize<TMessage>(data, _kafkaSerializationSettings.JsonSerializerOptions) 
               ?? throw new SerializationException("Cannot serialize given message");
    }
}