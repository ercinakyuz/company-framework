using System.Runtime.Serialization;
using System.Text.Json;
using Company.Framework.Messaging.Consumer;
using Company.Framework.Messaging.RabbitMq.Consumer.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Company.Framework.Messaging.RabbitMq.Consumer
{
    public abstract class RabbitConsumer<TMessage> : IConsumer
    {
        private readonly IModel _model;
        private readonly EventingBasicConsumer _consumer;
        private readonly RabbitConsumerSettings _settings;

        protected RabbitConsumer(IModel model, RabbitConsumerSettings settings)
        {
            _settings = settings;
            _model = model;
            _model.ExchangeDeclare(settings.Exchange, "topic");
            _model.QueueDeclare(settings.Queue);
            _model.QueueBind(settings.Queue, settings.Exchange, settings.Routing);
            _consumer = new EventingBasicConsumer(_model);
        }

        public Task SubscribeAsync(CancellationToken cancellationToken)
        {
            _consumer.Received += (_, args) =>
            {
                var message = JsonSerializer.Deserialize<TMessage>(args.Body.ToArray()) 
                              ?? throw new SerializationException("Cannot serialize given message");
                ConsumeAsync(message, cancellationToken).ConfigureAwait(false);
            };
            _model.BasicConsume(_settings.Queue, true, _consumer);
            return Task.CompletedTask;
        }

        public void Unsubscribe()
        {
            _model.Close();
        }

        public abstract Task ConsumeAsync(TMessage message, CancellationToken cancellationToken);

    }
}
