using Company.Framework.Messaging.Consumer;
using Confluent.Kafka;

namespace Company.Framework.Messaging.Kafka.Consumer
{
    public abstract class KafkaConsumer<TMessage> : IConsumer
    {
        private readonly IConsumer<Null, TMessage> _consumer;

        private readonly KafkaConsumerSettings _settings;

        protected KafkaConsumer(IConsumer<Null, TMessage> consumer, KafkaConsumerSettings settings)
        {
            _consumer = consumer;
            _settings = settings;
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
