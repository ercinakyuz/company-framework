using Company.Framework.Messaging.Producer.Context;

namespace Company.Framework.Messaging.RabbitMq.Producer.Context;

public interface IRabbitProducerContext : IProducerContext<IRabbitProducer>
{
}