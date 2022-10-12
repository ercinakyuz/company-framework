using Company.Framework.Messaging.Kafka.Model;
using Company.Framework.Messaging.Kafka.Producer.Args;
using Company.Framework.Messaging.Kafka.Producer.Context;
using Company.Framework.Messaging.Kafka.Producer.Settings;
using Confluent.Kafka;

namespace Company.Framework.Messaging.Kafka.Producer
{
    public class KafkaProducer : IKafkaProducer
    {
        public string BusName { get; }
        public string Name { get; }

        private readonly IProducer<Null, object> _producer;

        public KafkaProducer(KafkaProducerSettings settings, IProducer<Null, object> producer)
        {
            _producer = producer;
            (Name, BusName) = settings;
        }

        public async Task ProduceAsync(KafkaProduceArgs args, CancellationToken cancellationToken)
        {
            await _producer.ProduceAsync(args.Topic, new Message<Null, object>
            {
                Value = args.Message,
                Headers = args.Headers as Headers
            }, cancellationToken).ConfigureAwait(false);
        }
    }

    public class KafkaProducer<TId, TMessage> : IKafkaProducer<TId, TMessage>
    {
        private readonly IProducer<TId, TMessage> _producer;

        private readonly TypedKafkaProducerSettings _settings;

        public string Name => _settings.Name;
        public string BusName => _settings.BusName;
        public TypedKafkaProducerKey Key => new(typeof(TId), typeof(TMessage));

        public KafkaProducer(TypedKafkaProducerSettings settings, IProducer<TId, TMessage> producer)
        {
            _producer = producer;
            _settings = settings;
        }

        public async Task ProduceAsync(KafkaProduceArgs<TId, TMessage> args, CancellationToken cancellationToken)
        {
            await _producer.ProduceAsync(_settings.Topic, new Message<TId, TMessage>
            {
                Key = args.Id,
                Value = args.Message,
                Headers = KafkaHeaders.To(args.Headers)
            }, cancellationToken).ConfigureAwait(false);
        }
    }
}