using Company.Framework.Messaging.Bus;
using Company.Framework.Messaging.Producer.Context;

namespace Company.Framework.Messaging.RabbitMq.Producer.Context
{
    public class RabbitProducerContext : CoreProducerContext<IRabbitProducer>, IRabbitProducerContext
    {
        public RabbitProducerContext(IEnumerable<IRabbitProducer> producers) : base(producers, BusType.Rabbit)
        {
        }
    }
}
