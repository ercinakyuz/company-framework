using System.Collections.Immutable;

namespace Company.Framework.Messaging.Kafka.Producer.Context;

public class TypedKafkaProducerContext : ITypedKafkaProducerContext
{

    private readonly IReadOnlyDictionary<TypedKafkaProducerKey, ITypedKafkaProducer> _producerDictionaryByType;

    public TypedKafkaProducerContext(IEnumerable<ITypedKafkaProducer> producers)
    {
        _producerDictionaryByType = producers.ToImmutableDictionary(producer => producer.Key);
    }

    public IKafkaProducer<TId, TMessage> Resolve<TId, TMessage>()
    {
        var typedKafkaProducerKey = new TypedKafkaProducerKey(typeof(TId), typeof(TMessage));

        if (!_producerDictionaryByType.TryGetValue(typedKafkaProducerKey, out var producer))
            throw new InvalidOperationException(
                $"No available typed kafka producer for given type of: {typedKafkaProducerKey}");
        return (IKafkaProducer<TId, TMessage>)producer;
    }
}