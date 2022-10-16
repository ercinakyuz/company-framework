using System.Runtime.Serialization;
using System.Text.Json;
using Company.Framework.Messaging.Consumer;
using Company.Framework.Messaging.Consumer.Retrying.Args;
using Company.Framework.Messaging.RabbitMq.Consumer.Context;
using Company.Framework.Messaging.RabbitMq.Consumer.Retrying.Handler;
using Company.Framework.Messaging.RabbitMq.Consumer.Settings;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Company.Framework.Messaging.RabbitMq.Consumer
{
    public abstract class CoreRabbitConsumer<TMessage> : IConsumer
    {
        private readonly RabbitConsumerSettings _settings;
        private readonly IConnection _connection;
        private readonly ILogger _logger;
        private readonly IRabbitConsumerRetryingHandler? _retryingHandler;
        private readonly bool _isRetriable;

        protected CoreRabbitConsumer(IRabbitConsumerContext context, ILogger logger)
        {
            _settings = context.Settings;
            _connection = context.ConnectionContext.Resolve<IConnection>();
            _retryingHandler = context.RetryingHandler;
            _isRetriable = _retryingHandler != default;
            _logger = logger;
        }

        public async Task SubscribeAsync(CancellationToken cancellationToken)
        {
            await SubscribeToQueue(_settings.Declaration, cancellationToken);
            if (_isRetriable)
                await SubscribeToQueue(_retryingHandler!.DeclarationArgs, cancellationToken);
        }

        public void Unsubscribe()
        {
            _connection.Close();
        }

        protected abstract Task ConsumeAsync(TMessage message, CancellationToken cancellationToken);

        private async Task OnMessage(BasicDeliverEventArgs args, CancellationToken cancellationToken)
        {
            var message = JsonSerializer.Deserialize<TMessage>(args.Body.ToArray()) ?? throw new SerializationException("Cannot serialize given message");
            var headers = args.BasicProperties.Headers;
            try
            {
                await ConsumeAsync(message, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                if (_isRetriable)
                    await Task.Run(() =>
                        _retryingHandler!.HandleAsync(new ConsumerRetrialArgs(message, headers, exception.GetType()), cancellationToken)
                            .ConfigureAwait(false), cancellationToken);
            }

            await Task.Yield();

        }

        private Task SubscribeToQueue(RabbitDeclarationArgs declarationArgs, CancellationToken cancellationToken)
        {
            var model = _connection.CreateModel();
            var (exchange, routing, queue) = declarationArgs;
            var (exchangeName, exchangeType) = exchange;
            model.ExchangeDeclare(exchangeName, exchangeType);
            model.QueueDeclare(queue,true);
            model.QueueBind(queue, exchangeName, routing);
            var consumer = new AsyncEventingBasicConsumer(model);
            consumer.Received += (_, args) => OnMessage(args, cancellationToken);
            model.BasicConsume(declarationArgs.Queue, true, consumer);
            return Task.CompletedTask;
        }
    }
}
