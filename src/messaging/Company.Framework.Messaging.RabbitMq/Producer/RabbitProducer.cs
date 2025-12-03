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

        public async Task ProduceAsync(RabbitProduceArgs args, CancellationToken cancellationToken)
        {
            var (exchange, routing, message, headers) = args;
            var (exchangeName, exchangeType) = exchange;

            using var channel = await _connection.CreateChannelAsync();

            var basicProperties = new BasicProperties
            {
                Headers = headers!
            };
            await channel.ExchangeDeclareAsync(exchangeName, exchangeType);
            await channel.BasicPublishAsync(exchangeName, routing, false, basicProperties, _jsonSerializer.SerializeToUtf8Bytes(message));
        }
    }
}
