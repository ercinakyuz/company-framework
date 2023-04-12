using Company.Framework.Messaging.Sqs.Producer.Context;

namespace Company.Framework.Messaging.Sqs.Producer.Provider;

public class SqsProducerContextProvider : ISqsProducerContextProvider
{
    private readonly IReadOnlyDictionary<string, IEnumerable<ISqsProducer>> _producersDictionaryByBus;


    public SqsProducerContextProvider(IEnumerable<ISqsProducer> producers)
    {
        _producersDictionaryByBus = producers.GroupBy(producer => producer.BusName).ToDictionary(grouping => grouping.Key, grouping => grouping.AsEnumerable());
    }

    public ISqsProducerContext Resolve(string busName)
    {
        if (!_producersDictionaryByBus.TryGetValue(busName, out var producers))
            throw new InvalidOperationException(
                $"No available rabbit producers for given bus name of '{busName}'");
        return new SqsProducerContext(producers);
    }
}