using Company.Framework.Messaging.Consumer.Context;

namespace Company.Framework.Messaging.Kafka.Consumer.Context;

public interface IKafkaConsumerContext : IConsumerContext
{
    KafkaConsumerSettings Settings { get; }
    TConsumer Resolve<TConsumer>();
}