using Company.Framework.Messaging.Producer;
using Company.Framework.Messaging.RabbitMq.Producer.Args;

namespace Company.Framework.Messaging.RabbitMq.Producer;

public interface IRabbitProducer : IProducer<RabbitProduceArgs>
{
}