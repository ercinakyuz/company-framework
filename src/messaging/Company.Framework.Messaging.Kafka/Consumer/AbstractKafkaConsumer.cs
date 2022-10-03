using Company.Framework.Messaging.Consumer;
using Company.Framework.Messaging.Kafka.Consumer.Context;
using Confluent.Kafka;

namespace Company.Framework.Messaging.Kafka.Consumer
{
    public abstract class AbstractKafkaConsumer<TMessage> : IConsumer
    {
        private readonly IConsumer<Null, TMessage> _consumer;

        private readonly KafkaConsumerSettings _settings;

        protected AbstractKafkaConsumer(IKafkaConsumerContext consumerContext)
        {
            _consumer = consumerContext.Resolve<IConsumer<Null, TMessage>>();
            _settings = consumerContext.Settings;
        }

        public async Task SubscribeAsync(CancellationToken cancellationToken)
        {
            _consumer.Subscribe(_settings.Topic);
            while (!cancellationToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(cancellationToken);
                await ConsumeAsync(consumeResult.Message.Value, cancellationToken).ConfigureAwait(false);
            }
        }

        public void Unsubscribe()
        {
            _consumer.Close();
        }

        public abstract Task ConsumeAsync(TMessage message, CancellationToken cancellationToken);

    }
}
