using Company.Framework.Core.Serialization;

namespace Company.Framework.Messaging.Kafka.Serialization;

public class KafkaDefaultSerializer : KafkaMessageSerializer<object>
{
    public KafkaDefaultSerializer(IJsonSerializer serializer) : base(serializer)
    {
    }
}