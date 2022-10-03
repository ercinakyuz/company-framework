using Company.Framework.Messaging.RabbitMq.Bus;
using Company.Framework.Messaging.RabbitMq.Producer.Args;
using RabbitMQ.Client;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Company.Framework.Messaging.RabbitMq.Producer
{
    public class RabbitProducer : IRabbitProducer
    {
        public string Name { get; }

        private readonly IConnection _connection;

        public RabbitProducer(string name, IRabbitBus bus)
        {
            Name = name;
            _connection = bus.GetConnection<IConnection>();
        }

        public Task ProduceAsync(RabbitProduceArgs args, CancellationToken cancellationToken)
        {
            var model = _connection.CreateModel();
            var (exchange, routing, message) = args;
            var (exchangeName, exchangeType) = exchange;
            model.ExchangeDeclare(exchangeName, exchangeType);
            model.BasicPublish(exchangeName, routing, false, null, JsonSerializer.SerializeToUtf8Bytes(message));
            return Task.CompletedTask;
        }
    }
}
