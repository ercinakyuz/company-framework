using System.Net;
using Amazon.SQS;
using Amazon.SQS.Model;
using Company.Framework.Core.Serialization;
using Company.Framework.Messaging.Consumer;
using Company.Framework.Messaging.Consumer.Retrying.Args;
using Company.Framework.Messaging.Sqs.Consumer.Context;
using Company.Framework.Messaging.Sqs.Consumer.Retrying.Handler;
using Company.Framework.Messaging.Sqs.Consumer.Settings;
using Microsoft.Extensions.Logging;

namespace Company.Framework.Messaging.Sqs.Consumer
{
    public abstract class CoreSqsConsumer<TMessage> : IConsumer
    {
        private readonly SqsConsumerSettings _settings;
        private readonly SqsConsumerSettings? _retryingSettings;
        private readonly IAmazonSQS _client;
        private readonly ILogger _logger;
        private readonly ISqsConsumerRetryingHandler? _retryingHandler;
        private readonly IJsonSerializer _jsonSerializer;

        private Func<SqsConsumerSettings, CancellationToken, Task> SubscriptionDelegate =>
            (settings, ct) => Task.Run(() => SubscribeToQueue(settings, ct).ConfigureAwait(false));

        protected CoreSqsConsumer(ISqsConsumerContext context, ILogger logger)
        {
            _settings = context.Settings;
            _client = context.ClientContext.Resolve<IAmazonSQS>();
            _jsonSerializer = context.JsonSerializer;
            _logger = logger;
            _retryingHandler = context.RetryingHandler;
            _retryingSettings = _retryingHandler?.ConsumerSettings;
        }

        public async Task SubscribeAsync(CancellationToken cancellationToken)
        {
            await SubscriptionDelegate(_settings, cancellationToken).ConfigureAwait(false);
            if (_retryingSettings != default) await SubscriptionDelegate(_retryingSettings, cancellationToken).ConfigureAwait(false);
        }

        public void Unsubscribe()
        {
        }

        protected abstract Task ConsumeAsync(TMessage message, CancellationToken cancellationToken);


        private async Task SubscribeToQueue(SqsConsumerSettings settings, CancellationToken cancellationToken)
        {
            var (queue, concurrency) = settings;
            var queueUrl = await GetQueueUrlAsync(queue, cancellationToken);

            while (!cancellationToken.IsCancellationRequested)
            {
                var receiveMessageResponse = await _client.ReceiveMessageAsync(new ReceiveMessageRequest
                {
                    QueueUrl = queueUrl,
                    MaxNumberOfMessages = concurrency
                }, cancellationToken).ConfigureAwait(false);

                if (receiveMessageResponse is null || receiveMessageResponse.HttpStatusCode != HttpStatusCode.OK || !receiveMessageResponse.Messages.Any())
                    continue;

                var deleteMessageBatchRequestEntries = new List<DeleteMessageBatchRequestEntry>();

                foreach (var message in receiveMessageResponse.Messages)
                {
                    await _client.DeleteMessageAsync(new DeleteMessageRequest
                    {
                        QueueUrl = queueUrl,
                        ReceiptHandle = message.ReceiptHandle
                    }, cancellationToken).ConfigureAwait(false);

                    await TryConsumeAsync(message, cancellationToken).ConfigureAwait(false);
                }
            }
        }

        private async Task<string> GetQueueUrlAsync(string queue, CancellationToken cancellationToken)
        {
            var getQueueUrlResponse = await _client.GetQueueUrlAsync(new GetQueueUrlRequest(queue), cancellationToken).ConfigureAwait(false);
            if (getQueueUrlResponse is null || getQueueUrlResponse.HttpStatusCode != HttpStatusCode.OK)
                throw new InvalidOperationException($"{queue} is not available for subscription");
            return getQueueUrlResponse.QueueUrl;
        }

        private async Task TryConsumeAsync(Message message, CancellationToken cancellationToken)
        {
            var typedMessage = _jsonSerializer.Deserialize<TMessage>(message.Body);

            if (typedMessage is null)
            {
                _logger.LogError("Schema is not compatible with message: {}", message.Body);
                return;
            }

            try
            {
                await ConsumeAsync(typedMessage, cancellationToken);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                await Task.Run(() => _retryingHandler?.HandleAsync(new ConsumerRetrialArgs(typedMessage, message.MessageAttributes, exception.GetType()), cancellationToken)
                        .ConfigureAwait(false), cancellationToken);
            }
        }

    }
}
