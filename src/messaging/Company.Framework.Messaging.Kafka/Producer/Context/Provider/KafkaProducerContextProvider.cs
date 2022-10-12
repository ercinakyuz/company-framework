namespace Company.Framework.Messaging.Kafka.Producer.Context.Provider;

public class KafkaProducerContextProvider : IKafkaProducerContextProvider
{
    private readonly IReadOnlyDictionary<string, IEnumerable<IKafkaProducer>> _producersDictionaryByBus;


    public KafkaProducerContextProvider(IEnumerable<IKafkaProducer> producers)
    {
        _producersDictionaryByBus = producers.GroupBy(producer => producer.BusName).ToDictionary(grouping => grouping.Key, grouping => grouping.AsEnumerable());
    }

    public IKafkaProducerContext Resolve(string busName)
    {
        if (!_producersDictionaryByBus.TryGetValue(busName, out var producers))
            throw new InvalidOperationException(
                $"No available kafka producers for given bus name of: {busName}");
        return new KafkaProducerContext(producers);
    }
}