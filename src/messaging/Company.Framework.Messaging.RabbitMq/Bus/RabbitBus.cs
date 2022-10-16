using Company.Framework.Messaging.RabbitMq.Producer.Context;
using Company.Framework.Messaging.RabbitMq.Producer.Provider;

namespace Company.Framework.Messaging.RabbitMq.Bus;

public class RabbitBus : IRabbitBus
{
    public string Name { get; }
    public IRabbitProducerContext ProducerContext { get; }

    public RabbitBus(string name, IRabbitProducerContextProvider producerContextProvider)
    {
        Name = name;
        ProducerContext = producerContextProvider.Resolve(name);
    }
}