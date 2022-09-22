using Company.Framework.Messaging.Producer;
using Company.Framework.Messaging.Producer.Args;
using Confluent.Kafka;

namespace Company.Framework.Messaging.Kafka.Producer
{
    public class KafkaProducer : IProducer
    {
        private readonly IProducer<Null, object> _producer;

        public KafkaProducer(IProducer<Null, object> producer)
        {
            _producer = producer;
        }

        public async Task ProduceAsync<TMessage>(ProduceArgs<TMessage> args, CancellationToken cancellationToken) where TMessage : notnull
        {
            await _producer.ProduceAsync(args.Channel, new Message<Null, object>
            {
                Value = args.Message,
            }, cancellationToken);
        }
    }
}