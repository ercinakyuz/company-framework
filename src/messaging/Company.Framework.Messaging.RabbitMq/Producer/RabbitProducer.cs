using System.Collections.Concurrent;
using Company.Framework.Core.Serialization;
using Company.Framework.Messaging.RabbitMq.Connection.Context;
using Company.Framework.Messaging.RabbitMq.Producer.Args;
using RabbitMQ.Client;

namespace Company.Framework.Messaging.RabbitMq.Producer
{
    public class RabbitProducer : IRabbitProducer
    {
        public string BusName { get; }
        public string Name { get; }

        private readonly IConnection _connection;

        private readonly IJsonSerializer _jsonSerializer;

        public RabbitProducer(string name, string busName, IRabbitConnectionContext connectionContext, IJsonSerializer jsonSerializer)
        {
            _jsonSerializer = jsonSerializer;
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
                model.BasicPublish(exchangeName, routing, false, basicProperties, _jsonSerializer.SerializeToUtf8Bytes(message));
            }

            return Task.CompletedTask;
        }
    }
}
