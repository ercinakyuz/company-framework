using System.Runtime.Serialization;
using Company.Framework.Core.Serializer;
using Company.Framework.Messaging.Consumer;
using Company.Framework.Messaging.Consumer.Retrying.Args;
using Company.Framework.Messaging.RabbitMq.Consumer.Context;
using Company.Framework.Messaging.RabbitMq.Consumer.Extensions;
using Company.Framework.Messaging.RabbitMq.Consumer.Retrying.Handler;
using Company.Framework.Messaging.RabbitMq.Consumer.Settings;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Company.Framework.Messaging.RabbitMq.Consumer
{
    public abstract class CoreRabbitConsumer<TMessage> : IConsumer
    {
        private readonly IJsonSerializer _jsonSerializer;
        private readonly RabbitConsumerSettings _settings;
        private readonly IModel _mainModel;
        private readonly IModel? _retryingModel;
        private readonly ILogger _logger;
        private readonly IRabbitConsumerRetryingHandler? _retryingHandler;

        protected CoreRabbitConsumer(IRabbitConsumerContext context, ILogger logger)
        {
            _jsonSerializer = context.JsonSerializer;
            _settings = context.Settings;
            var connection = context.ConnectionContext.Resolve<IConnection>();
            _mainModel = connection.BuildModel(_settings.Declaration);
            _logger = logger;
            _retryingHandler = context.RetryingHandler;
            if (_retryingHandler is not null)
            {
                _retryingModel = connection.BuildModel(_retryingHandler.DeclarationArgs);
            }
        }

        public async Task SubscribeAsync(CancellationToken cancellationToken)
        {
            var subscriptionTasks = new List<Task>
            {
                _mainModel.SubscribeToQueue(OnMessage, _settings.Declaration.Queue, cancellationToken)
            };

            var retryingSubscription = _retryingModel?.SubscribeToQueue(OnMessage, _retryingHandler!.DeclarationArgs.Queue, cancellationToken);
            if (retryingSubscription is not null) subscriptionTasks.Add(retryingSubscription);

            await Task.WhenAll(subscriptionTasks);

        }

        public void Unsubscribe()
        {
            _mainModel.Close();
            _retryingModel?.Close();
        }

        protected abstract Task ConsumeAsync(TMessage message, CancellationToken cancellationToken);

        private async Task OnMessage(BasicDeliverEventArgs args, CancellationToken cancellationToken)
        {
            var message = _jsonSerializer.Deserialize<TMessage>(args.Body.ToArray()) ?? throw new SerializationException("Cannot serialize given message");
            var headers = args.BasicProperties.Headers;
            try
            {
                await ConsumeAsync(message, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                await Task.Run(() => _retryingHandler?.HandleAsync(new ConsumerRetrialArgs(message, headers, exception.GetType()), cancellationToken)
                    .ConfigureAwait(false), cancellationToken);
            }
        }
    }
}
