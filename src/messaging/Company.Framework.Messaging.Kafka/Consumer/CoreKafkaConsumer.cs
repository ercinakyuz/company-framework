using Company.Framework.Messaging.Consumer;
using Company.Framework.Messaging.Consumer.Retrying.Args;
using Company.Framework.Messaging.Kafka.Consumer.Context;
using Company.Framework.Messaging.Kafka.Consumer.Retrying.Handler;
using Company.Framework.Messaging.Kafka.Consumer.Settings;
using Company.Framework.Messaging.Kafka.Model;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Logging;

namespace Company.Framework.Messaging.Kafka.Consumer
{
    public abstract class CoreKafkaConsumer<TMessage> : CoreKafkaConsumer<Null, TMessage>
    {
        protected CoreKafkaConsumer(IKafkaConsumerContext<Null, TMessage> consumerContext, ILogger logger) : base(consumerContext, logger)
        {
        }
    }

    public abstract class CoreKafkaConsumer<TId, TMessage> : IConsumer
    {
        private readonly IConsumer<TId, TMessage> _consumer;

        private readonly KafkaConsumerSettings _settings;

        private readonly IKafkaConsumerRetryingHandler<TId, TMessage>? _retryingHandler;

        private readonly IAdminClient _adminClient;

        private readonly bool _hasRetrial;

        protected readonly ILogger Logger;


        protected CoreKafkaConsumer(IKafkaConsumerContext<TId, TMessage> consumerContext, ILogger logger)
        {
            _consumer = consumerContext.Resolve<IConsumer<TId, TMessage>>();
            _settings = consumerContext.Settings;
            _retryingHandler = consumerContext.RetryingHandler;
            _adminClient = consumerContext.AdminClientContext.Resolve<IAdminClient>();
            _hasRetrial = _retryingHandler != default;
            Logger = logger;
        }

        public async Task SubscribeAsync(CancellationToken cancellationToken)
        {
            await SubscribeToTopics();
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

        private async Task TryConsumeAsync(Message<TId, TMessage> message, CancellationToken cancellationToken)
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
                        _retryingHandler!.HandleAsync(new KafkaConsumerRetrialArgs<TId, TMessage>(message.Key, message.Value, KafkaHeaders.From(message.Headers), exception.GetType()), cancellationToken)
                        .ConfigureAwait(false), cancellationToken);
            }
        }

        private async Task SubscribeToTopics()
        {
            if (_hasRetrial)
            {
                var retryTopicSettings = _retryingHandler!.TopicSettings;
                try
                {
                    await _adminClient.CreateTopicsAsync(new[]
                    {
                        new TopicSpecification {
                            Name = retryTopicSettings.Name,
                            NumPartitions = retryTopicSettings.Partition,
                            ReplicationFactor = retryTopicSettings.Replication
                        }
                    }).ConfigureAwait(false);
                }
                catch (CreateTopicsException exception)
                {
                    if (exception.Results.Any(result => result.Error.Code != ErrorCode.TopicAlreadyExists))
                        throw;
                }

                _consumer.Subscribe(new[] { _settings.Topic, retryTopicSettings.Name });
            }

            else
                _consumer.Subscribe(_settings.Topic);
        }

    }
}
