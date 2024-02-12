using Company.Framework.Messaging.Kafka.AdminClient.Context;
using Company.Framework.Messaging.Kafka.Consumer.Retrying.Handler;
using Company.Framework.Messaging.Kafka.Consumer.Settings;
using Confluent.Kafka;

namespace Company.Framework.Messaging.Kafka.Consumer.Context
{
    public class KafkaConsumerContext<TId, TMessage> : IKafkaConsumerContext<TId, TMessage>
    {
        private readonly IConsumer<TId, TMessage> _consumer;

        public KafkaConsumerSettings Settings { get; }
        public IKafkaConsumerRetryingHandler<TId, TMessage>? RetryingHandler { get; }
        public IKafkaAdminClientContext AdminClientContext { get; }

        public KafkaConsumerContext(IConsumer<TId, TMessage> consumer, KafkaConsumerSettings settings, IKafkaAdminClientContext adminClientContext, IKafkaConsumerRetryingHandler<TId,TMessage>? retryingHandler = default)
        {
            _consumer = consumer;
            Settings = settings;
            AdminClientContext = adminClientContext;
            RetryingHandler = retryingHandler;
        }

        public TConsumer Resolve<TConsumer>()
        {
            if (typeof(TConsumer) != typeof(IConsumer<TId, TMessage>))
                throw new InvalidOperationException("Consumer type is not valid");
            return (TConsumer)_consumer;
        }

        public IKafkaConsumerRetryingHandler<TId, TMessage>? ResolveRetringHandler()
        {
            throw new NotImplementedException();
        }
    }
}
