using Company.Framework.Messaging.Bus;
using Company.Framework.Messaging.RabbitMq.Producer.Context;

namespace Company.Framework.Messaging.RabbitMq.Bus
{
    public interface IRabbitBus : IBus
    {
        IRabbitProducerContext ProducerContext { get; }
    }
}
