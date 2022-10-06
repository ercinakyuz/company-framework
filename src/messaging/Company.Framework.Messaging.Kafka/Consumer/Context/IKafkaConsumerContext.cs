using Company.Framework.Messaging.Consumer.Context;
using Company.Framework.Messaging.Kafka.Consumer.Retrial.Context;
using Company.Framework.Messaging.Kafka.Consumer.Settings;

namespace Company.Framework.Messaging.Kafka.Consumer.Context;

public interface IKafkaConsumerContext : IConsumerContext
{
    KafkaConsumerSettings Settings { get; }
    IKafkaRetrialContext? Retrial { get; }
    TConsumer Resolve<TConsumer>();

}