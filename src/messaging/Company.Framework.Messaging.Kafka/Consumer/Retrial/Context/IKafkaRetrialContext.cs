using Company.Framework.Messaging.Consumer.Retrial;
using Company.Framework.Messaging.Kafka.Consumer.Retrial.Context.Args;

namespace Company.Framework.Messaging.Kafka.Consumer.Retrial.Context;

public interface IKafkaRetrialContext : IRetrialContext<KafkaRetryArgs>
{
    string Topic { get; }
}