using System.Runtime.Serialization;
using System.Text.Json;
using Company.Framework.Messaging.Consumer;
using Company.Framework.Messaging.RabbitMq.Bus;
using Company.Framework.Messaging.RabbitMq.Consumer.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Company.Framework.Messaging.RabbitMq.Consumer
{
    public abstract class AbstractRabbitConsumer<TMessage> : IConsumer
    {
        private readonly EventingBasicConsumer _consumer;
        private readonly RabbitConsumerSettings _settings;
        private readonly IModel _model;

        protected AbstractRabbitConsumer(IRabbitBus bus, RabbitConsumerSettings settings)
        {
            _settings = settings;
            _model = bus.GetConnection<IConnection>().CreateModel();
            var (exchange, routing, queue) = settings;
            var (exchangeName, exchangeType) = exchange;
            _model.ExchangeDeclare(exchangeName, exchangeType);
            _model.QueueDeclare(queue);
            _model.QueueBind(queue, exchangeName, routing);
            _consumer = new EventingBasicConsumer(_model);
        }

        public Task SubscribeAsync(CancellationToken cancellationToken)
        {
            _consumer.Received += async (_, args) => await OnMessage(args, cancellationToken);
            _model.BasicConsume(_settings.Queue, true, _consumer);
            return Task.CompletedTask;
        }

        public void Unsubscribe()
        {
            _model.Close();
        }

        protected abstract Task ConsumeAsync(TMessage message, CancellationToken cancellationToken);

        private async Task OnMessage(BasicDeliverEventArgs args, CancellationToken cancellationToken)
        {
            var message = JsonSerializer.Deserialize<TMessage>(args.Body.ToArray()) ?? throw new SerializationException("Cannot serialize given message");
            await ConsumeAsync(message, cancellationToken).ConfigureAwait(false);
        }
    }
}
