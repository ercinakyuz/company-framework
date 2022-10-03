using Company.Framework.Messaging.Bus;

namespace Company.Framework.Messaging.Producer.Context
{
    public abstract class CoreProducerContext<TProducer> : IProducerContext where TProducer : IProducer
    {
        private readonly IReadOnlyDictionary<string, TProducer> _producerDictionary;

        protected CoreProducerContext(IEnumerable<TProducer> producers)
        {
            _producerDictionary = producers.ToDictionary(producer => producer.Name);
        }

        public TProducer Resolve(string name)
        {
            return _producerDictionary.TryGetValue(name, out var producer)
                ? producer
                : throw new InvalidOperationException($"No available producer for name: {name}");
        }
    }
}
