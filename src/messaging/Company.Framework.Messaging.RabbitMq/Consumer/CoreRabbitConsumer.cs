using Company.Framework.Core.Serialization;
using Company.Framework.Messaging.Consumer;
using Company.Framework.Messaging.Consumer.Retrying.Args;
using Company.Framework.Messaging.RabbitMq.Connection.Context;
using Company.Framework.Messaging.RabbitMq.Consumer.Context;
using Company.Framework.Messaging.RabbitMq.Consumer.Extensions;
using Company.Framework.Messaging.RabbitMq.Consumer.Retrying.Handler;
using Company.Framework.Messaging.RabbitMq.Consumer.Settings;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Runtime.Serialization;

namespace Company.Framework.Messaging.RabbitMq.Consumer
{
    public abstract class CoreRabbitConsumer<TMessage> : IConsumer
    {
        private readonly IJsonSerializer _jsonSerializer;
        private readonly RabbitConsumerSettings _settings;
        private readonly IRabbitConnectionContext _connectionContext;
        private readonly ILogger _logger;
        private readonly IRabbitConsumerRetryingHandler? _retryingHandler;
        private IChannel? _mainChannel;
        private IChannel? _retryingChannel;

        protected CoreRabbitConsumer(IRabbitConsumerContext context, ILogger logger)
        {
            _jsonSerializer = context.JsonSerializer;
            _settings = context.Settings;
            _connectionContext = context.ConnectionContext;
            _logger = logger;
            _retryingHandler = context.RetryingHandler;

        }

        public async Task SubscribeAsync(CancellationToken cancellationToken)
        {
            var connection = await _connectionContext.ResolveAsync<IConnection>(cancellationToken);
            _mainChannel = await connection.BuildChannelAsync(_settings.Declaration);
            var subscriptionTasks = new List<Task>
            {
                _mainChannel.SubscribeToQueueAsync(OnMessage, _settings.Declaration.Queue, cancellationToken)
            };
            if (_retryingHandler is not null)
            {
                _retryingChannel = await connection.BuildChannelAsync(_retryingHandler.DeclarationArgs);
                var retryingSubscription = _retryingChannel.SubscribeToQueueAsync(OnMessage, _retryingHandler!.DeclarationArgs.Queue, cancellationToken);
                if (retryingSubscription is not null) subscriptionTasks.Add(retryingSubscription);
            }
            await Task.WhenAll(subscriptionTasks);

        }

        public async Task UnsubscribeAsync()
        {
            var tasks = new List<Task>();
            if (_mainChannel is not null)
            {
                tasks.Add(_mainChannel.CloseAsync());
            }
            if (_retryingChannel is not null)
            {
                tasks.Add(_retryingChannel.CloseAsync());
            }
            await Task.WhenAll(tasks);
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
