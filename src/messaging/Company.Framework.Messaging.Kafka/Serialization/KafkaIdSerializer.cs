using Company.Framework.Core.Id.Abstractions;
using Confluent.Kafka;

namespace Company.Framework.Messaging.Kafka.Serialization;

public class KafkaIdSerializer<TId> : ISerializer<TId> where TId : IId<TId>
{
    public byte[] Serialize(TId id, SerializationContext context)
    {
        return id.GetBytes();
    }
}