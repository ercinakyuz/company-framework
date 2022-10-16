using Company.Framework.Messaging.RabbitMq.Producer.Context;

namespace Company.Framework.Messaging.RabbitMq.Producer.Provider;

public class RabbitProducerContextProvider : IRabbitProducerContextProvider
{
    private readonly IReadOnlyDictionary<string, IEnumerable<IRabbitProducer>> _producersDictionaryByBus;


    public RabbitProducerContextProvider(IEnumerable<IRabbitProducer> producers)
    {
        _producersDictionaryByBus = producers.GroupBy(producer => producer.BusName).ToDictionary(grouping => grouping.Key, grouping => grouping.AsEnumerable());
    }

    public IRabbitProducerContext Resolve(string busName)
    {
        if (!_producersDictionaryByBus.TryGetValue(busName, out var producers))
            throw new InvalidOperationException(
                $"No available rabbit producers for given bus name of '{busName}'");
        return new RabbitProducerContext(producers);
    }
}