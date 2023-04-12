using Company.Framework.Messaging.Sqs.Producer.Context;

namespace Company.Framework.Messaging.Sqs.Producer.Provider;

public interface ISqsProducerContextProvider
{
    ISqsProducerContext Resolve(string busName);
}