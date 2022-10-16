using Company.Framework.Messaging.Consumer.Context;
using Company.Framework.Messaging.Kafka.AdminClient.Context;
using Company.Framework.Messaging.Kafka.Consumer.Retrying.Handler;
using Company.Framework.Messaging.Kafka.Consumer.Settings;

namespace Company.Framework.Messaging.Kafka.Consumer.Context;

public interface IKafkaConsumerContext : IConsumerContext
{
    KafkaConsumerSettings Settings { get; }
    IKafkaConsumerRetryingHandler? RetrialContext { get; }
    IKafkaAdminClientContext AdminClientContext { get; }
    TConsumer Resolve<TConsumer>();

}