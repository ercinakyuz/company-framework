namespace Company.Framework.Messaging.Kafka.Producer.Context.Provider;

public class TypedKafkaProducerContextProvider : ITypedKafkaProducerContextProvider
{

    private readonly IReadOnlyDictionary<string, IEnumerable<ITypedKafkaProducer>> _producersDictionaryByBus;


    public TypedKafkaProducerContextProvider(IEnumerable<ITypedKafkaProducer> producers)
    {
        _producersDictionaryByBus = producers.GroupBy(producer => producer.BusName).ToDictionary(grouping => grouping.Key, grouping => grouping.AsEnumerable());
    }

    public ITypedKafkaProducerContext Resolve(string busName)
    {
        if (!_producersDictionaryByBus.TryGetValue(busName, out var producers))
            throw new InvalidOperationException(
                $"No available typed kafka producers for given bus name of: {busName}");
        return new TypedKafkaProducerContext(producers);
    }
}