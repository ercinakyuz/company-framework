using Company.Framework.Messaging.RabbitMq.Producer.Args;
using RabbitMQ.Client;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Company.Framework.Messaging.RabbitMq.Producer
{
    public class RabbitProducer : IRabbitProducer
    {
        public string Name { get; }

        private readonly IModel _model;

        public RabbitProducer(string name, IModel model)
        {
            Name = name;
            _model = model;
        }

        public Task ProduceAsync(RabbitProduceArgs args, CancellationToken cancellationToken)
        {
            _model.ExchangeDeclare(args.Exchange, "topic");
            _model.BasicPublish(args.Exchange, args.Routing, false, null, JsonSerializer.SerializeToUtf8Bytes(args.Message));
            return Task.CompletedTask;
        }
    }
}
