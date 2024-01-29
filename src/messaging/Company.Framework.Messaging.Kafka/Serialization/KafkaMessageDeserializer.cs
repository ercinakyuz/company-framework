using System.Runtime.Serialization;
using Company.Framework.Core.Serialization;
using Confluent.Kafka;

namespace Company.Framework.Messaging.Kafka.Serialization;

public class KafkaMessageDeserializer<TMessage> : IDeserializer<TMessage>
{
    private readonly IJsonSerializer _jsonSerializer;

    public KafkaMessageDeserializer(IJsonSerializer jsonSerializer)
    {
        _jsonSerializer = jsonSerializer;
    }

    public TMessage Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        if (isNull)
            throw new SerializationException("Byte array of message cannot be null");
        return _jsonSerializer.Deserialize<TMessage>(data) ?? throw new SerializationException("Cannot serialize given message");
    }
}
