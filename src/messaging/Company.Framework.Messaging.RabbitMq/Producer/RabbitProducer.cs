using System.Collections.Concurrent;
using Company.Framework.Messaging.RabbitMq.Connection.Context;
using Company.Framework.Messaging.RabbitMq.Producer.Args;
using RabbitMQ.Client;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Company.Framework.Messaging.RabbitMq.Producer
{
    public class RabbitProducer : IRabbitProducer
    {
        public string BusName { get; }
        public string Name { get; }

        private readonly IConnection _connection;

        public RabbitProducer(string name, string busName, IRabbitConnectionContext connectionContext)
        {
            Name = name;
            BusName = busName;
            _connection = connectionContext.Resolve<IConnection>();
        }

        public Task ProduceAsync(RabbitProduceArgs args, CancellationToken cancellationToken)
        {
            var (exchange, routing, message, headers) = args;
            var (exchangeName, exchangeType) = exchange;

            using (var model = _connection.CreateModel())
            {
                var basicProperties = model.CreateBasicProperties();
                basicProperties.Headers ??= new ConcurrentDictionary<string, object>();
                
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        basicProperties.Headers.Add(header.Key, header.Value);
                    }
                }
                model.ExchangeDeclare(exchangeName, exchangeType);
                model.BasicPublish(exchangeName, routing, false, basicProperties, JsonSerializer.SerializeToUtf8Bytes(message));
            }

            return Task.CompletedTask;
        }
    }
}
