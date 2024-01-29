using Company.Framework.Core.Serialization;
using Confluent.Kafka;

namespace Company.Framework.Messaging.Kafka.Serialization;

public class KafkaMessageSerializer<TMessage> : ISerializer<TMessage> where TMessage : notnull
{
    private readonly IJsonSerializer _serializer;

    public KafkaMessageSerializer(IJsonSerializer serializer)
    {
        _serializer = serializer;
    }

    public byte[] Serialize(TMessage message, SerializationContext context)
    {
        return _serializer.SerializeToUtf8Bytes(message);
    }
}
