using System.Net;
using Amazon.SQS;
using Amazon.SQS.Model;
using Company.Framework.Core.Serialization;
using Company.Framework.Messaging.Consumer;
using Company.Framework.Messaging.Sqs.Consumer.Context;
using Company.Framework.Messaging.Sqs.Consumer.Settings;
using Microsoft.Extensions.Logging;

namespace Company.Framework.Messaging.Sqs.Consumer
{
    public abstract class CoreSqsConsumer<TMessage> : IConsumer
    {

        private static readonly List<string> AttributeNames = new() { "All" };

        private readonly SqsConsumerSettings _settings;
        private readonly IAmazonSQS _client;
        private readonly ILogger _logger;
        private readonly IJsonSerializer _jsonSerializer;


        protected CoreSqsConsumer(ISqsConsumerContext context, ILogger logger)
        {
            _settings = context.Settings;
            _client = context.ClientContext.Resolve<IAmazonSQS>();
            _jsonSerializer = context.JsonSerializer;
            _logger = logger;
        }

        public async Task SubscribeAsync(CancellationToken cancellationToken)
        {
            await SubscribeToQueue(_settings, cancellationToken).ConfigureAwait(false);
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
                    AttributeNames = AttributeNames,
                    MessageAttributeNames = AttributeNames,
                    MaxNumberOfMessages = concurrency,

                }, cancellationToken).ConfigureAwait(false);

                if (receiveMessageResponse is null || receiveMessageResponse.HttpStatusCode != HttpStatusCode.OK || !receiveMessageResponse.Messages.Any())
                    continue;

                foreach (var message in receiveMessageResponse.Messages)
                {
                    if (await TryConsumeAsync(message, queueUrl, cancellationToken).ConfigureAwait(false))
                    {
                        await _client.DeleteMessageAsync(new DeleteMessageRequest
                        {
                            QueueUrl = queueUrl,
                            ReceiptHandle = message.ReceiptHandle
                        }, cancellationToken).ConfigureAwait(false);
                    }
                }
            }
        }

        private async Task<string> GetQueueUrlAsync(string queue, CancellationToken cancellationToken)
        {
            try
            {
                var getQueueUrlResponse = await _client.GetQueueUrlAsync(new GetQueueUrlRequest(queue), cancellationToken).ConfigureAwait(false);
                if (getQueueUrlResponse is null || getQueueUrlResponse.HttpStatusCode != HttpStatusCode.OK)
                    throw new InvalidOperationException($"{queue} is not available for subscription");
                return getQueueUrlResponse.QueueUrl;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


        }

        private async Task<bool> TryConsumeAsync(Message message, string queueUrl, CancellationToken cancellationToken)
        {
            var typedMessage = _jsonSerializer.Deserialize<TMessage>(message.Body);
            if (typedMessage is null)
            {
                _logger.LogError("Schema is not compatible with message: {}", message.Body);
                return true;
            }

            try
            {
                await ConsumeAsync(typedMessage, cancellationToken);
                return true;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                return false;
            }
        }
    }
}
