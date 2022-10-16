using Company.Framework.Messaging.RabbitMq.Producer.Context;

namespace Company.Framework.Messaging.RabbitMq.Producer.Provider;

public interface IRabbitProducerContextProvider
{
    IRabbitProducerContext Resolve(string busName);
}