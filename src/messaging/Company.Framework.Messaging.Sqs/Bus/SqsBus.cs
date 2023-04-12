using Company.Framework.Messaging.Sqs.Producer.Context;
using Company.Framework.Messaging.Sqs.Producer.Provider;

namespace Company.Framework.Messaging.Sqs.Bus;

public class SqsBus : ISqsBus
{
    public string Name { get; }
    public ISqsProducerContext ProducerContext { get; }

    public SqsBus(string name, ISqsProducerContextProvider producerContextProvider)
    {
        Name = name;
        ProducerContext = producerContextProvider.Resolve(name);
    }
}