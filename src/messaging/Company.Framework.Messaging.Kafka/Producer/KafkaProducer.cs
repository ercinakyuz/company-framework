using Company.Framework.Messaging.Kafka.Producer.Args;
using Confluent.Kafka;

namespace Company.Framework.Messaging.Kafka.Producer
{
    public class KafkaProducer : IKafkaProducer
    {
        public string Name { get; }

        private readonly IProducer<Null, object> _producer;

        public KafkaProducer(string name, IProducer<Null, object> producer)
        {
            _producer = producer;
            Name = name;
        }

        public async Task ProduceAsync(KafkaProduceArgs args, CancellationToken cancellationToken)
        {
            await _producer.ProduceAsync(args.Topic, new Message<Null, object>
            {
                Value = args.Message,
            }, cancellationToken);
        }
    }
}