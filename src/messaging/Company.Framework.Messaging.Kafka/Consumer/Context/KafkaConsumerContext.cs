using Company.Framework.Messaging.Kafka.AdminClient.Context;
using Company.Framework.Messaging.Kafka.Consumer.Retrying.Handler;
using Company.Framework.Messaging.Kafka.Consumer.Settings;
using Confluent.Kafka;

namespace Company.Framework.Messaging.Kafka.Consumer.Context
{
    public class KafkaConsumerContext<TId, TMessage> : IKafkaConsumerContext
    {
        private readonly IConsumer<TId, TMessage> _consumer;

        public KafkaConsumerSettings Settings { get; }
        public IKafkaConsumerRetryingHandler? RetrialContext { get; }
        public IKafkaAdminClientContext AdminClientContext { get; }

        public KafkaConsumerContext(IConsumer<TId, TMessage> consumer, KafkaConsumerSettings settings, IKafkaAdminClientContext adminClientContext, IKafkaConsumerRetryingHandler? consumerRetrialContext = default)
        {
            _consumer = consumer;
            Settings = settings;
            AdminClientContext = adminClientContext;
            RetrialContext = consumerRetrialContext;
        }

        public TConsumer Resolve<TConsumer>()
        {
            if (typeof(TConsumer) != typeof(IConsumer<TId, TMessage>))
                throw new InvalidOperationException("Consumer type is not valid");
            return (TConsumer)_consumer;
        }
    }
}
