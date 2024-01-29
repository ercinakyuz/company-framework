using Company.Framework.Core.Id.Abstractions;
using Confluent.Kafka;

namespace Company.Framework.Messaging.Kafka.Serialization;

public class KafkaIdDeserializer<TId> : IDeserializer<TId> where TId: IId<TId>
{
    public TId Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        return TId.FromBytes(data);
    }
}