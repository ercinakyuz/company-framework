using Company.Framework.Messaging.Consumer;
using Company.Framework.Messaging.Kafka.Consumer.Context;
using Company.Framework.Messaging.Kafka.Consumer.Retrial.Context;
using Company.Framework.Messaging.Kafka.Consumer.Retrial.Context.Args;
using Company.Framework.Messaging.Kafka.Consumer.Settings;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace Company.Framework.Messaging.Kafka.Consumer
{
    public abstract class CoreKafkaConsumer<TMessage> : IConsumer
    {
        private readonly IConsumer<Ignore, TMessage> _consumer;

        private readonly KafkaConsumerSettings _settings;

        private readonly IKafkaRetrialContext? _retrialContext;

        private readonly bool _hasRetrial;

        protected readonly ILogger Logger;


        protected CoreKafkaConsumer(IKafkaConsumerContext consumerContext, ILogger logger)
        {
            _consumer = consumerContext.Resolve<IConsumer<Ignore, TMessage>>();
            _settings = consumerContext.Settings;
            _retrialContext = consumerContext.Retrial;
            _hasRetrial = _retrialContext != default;
            Logger = logger;
        }

        public async Task SubscribeAsync(CancellationToken cancellationToken)
        {
            if (_hasRetrial)
                _consumer.Subscribe(new[] { _settings.Topic, _retrialContext!.Topic });
            else
                _consumer.Subscribe(_settings.Topic);
            while (!cancellationToken.IsCancellationRequested)
            {
                var message = _consumer.Consume(cancellationToken).Message;
                await TryConsumeAsync(message, cancellationToken).ConfigureAwait(false);
            }
        }

        public void Unsubscribe()
        {
            _consumer.Close();
        }

        public abstract Task ConsumeAsync(TMessage message, CancellationToken cancellationToken);

        private async Task TryConsumeAsync(Message<Ignore, TMessage> message, CancellationToken cancellationToken)
        {
            try
            {
                await ConsumeAsync(message.Value, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, exception.Message);
                if (_hasRetrial)
                    await Task.Run(() => 
                        _retrialContext!.RetryAsync(new KafkaRetryArgs(message.Value!, message.Headers, exception.GetType()), cancellationToken)
                        .ConfigureAwait(false), cancellationToken);
            }
        }

    }
}
