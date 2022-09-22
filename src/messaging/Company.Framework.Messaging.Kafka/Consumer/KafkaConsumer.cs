using Company.Framework.Messaging.Consumer;
using Confluent.Kafka;

namespace Company.Framework.Messaging.Kafka.Consumer
{
    public abstract class KafkaConsumer<TMessage> : IConsumer
    {
        protected abstract string Topic { get; }

        private readonly IConsumer<Null, TMessage> _consumer;


        protected KafkaConsumer(IConsumer<Null, TMessage> consumer)
        {
            _consumer = consumer;
        }

        public async Task SubscribeAsync(CancellationToken cancellationToken)
        {
            _consumer.Subscribe(Topic);
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
