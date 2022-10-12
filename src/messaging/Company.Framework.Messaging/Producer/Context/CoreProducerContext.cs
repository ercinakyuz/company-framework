using static Company.Framework.Messaging.Constant.MessagingConstants;

namespace Company.Framework.Messaging.Producer.Context
{
    public abstract class CoreProducerContext<TProducer> : IProducerContext<TProducer> where TProducer : IProducer
    {
        private readonly IReadOnlyDictionary<string, TProducer> _producerDictionary;


        protected CoreProducerContext(IEnumerable<TProducer> producers)
        {
            _producerDictionary = producers.ToDictionary(producer => producer.Name);
        }

        public TProducer Default()
        {
            return Resolve(DefaultProducerName);
        }

        public TProducer Resolve(string busName)
        {
            return _producerDictionary.TryGetValue(busName, out var producer)
                ? producer
                : throw new InvalidOperationException($"No available producer for busName: {busName}");
        }
    }
}
