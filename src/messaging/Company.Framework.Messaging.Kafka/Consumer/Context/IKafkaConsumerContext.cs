using Company.Framework.Messaging.Consumer.Context;
using Company.Framework.Messaging.Kafka.AdminClient.Context;
using Company.Framework.Messaging.Kafka.Consumer.Retrying.Handler;
using Company.Framework.Messaging.Kafka.Consumer.Settings;

namespace Company.Framework.Messaging.Kafka.Consumer.Context;

public interface IKafkaConsumerContext<TId, TMessage> : IConsumerContext
{
    KafkaConsumerSettings Settings { get; }
    IKafkaAdminClientContext AdminClientContext { get; }
    IKafkaConsumerRetryingHandler<TId, TMessage>? RetryingHandler { get; }
    TConsumer Resolve<TConsumer>();

}