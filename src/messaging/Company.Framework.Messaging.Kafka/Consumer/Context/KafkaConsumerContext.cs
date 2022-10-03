using Confluent.Kafka;

namespace Company.Framework.Messaging.Kafka.Consumer.Context
{
    public class KafkaConsumerContext<TMessage> : IKafkaConsumerContext
    {
        private readonly IConsumer<Null, TMessage> _consumer;

        public KafkaConsumerSettings Settings { get; }

        public KafkaConsumerContext(IConsumer<Null, TMessage> consumer, KafkaConsumerSettings settings)
        {
            _consumer = consumer;
            Settings = settings;
        }

        public TConsumer Resolve<TConsumer>()
        {
            if (typeof(TConsumer) != typeof(IConsumer<Null, TMessage>))
                throw new InvalidOperationException($"Consumer type is not valid");
            return (TConsumer)_consumer;
        }
    }
}
