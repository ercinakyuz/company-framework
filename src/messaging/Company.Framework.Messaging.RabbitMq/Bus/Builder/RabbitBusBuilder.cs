using Company.Framework.Messaging.Bus.Builder;
using Company.Framework.Messaging.RabbitMq.Producer.Context;

namespace Company.Framework.Messaging.RabbitMq.Bus.Builder;

public class RabbitBusBuilder : CoreBusBuilder<RabbitBusBuilder>
{
    public RabbitBusBuilder(MainBusServiceBuilder mainBusServiceBuilder) : base(mainBusServiceBuilder)
    {
    }
    public RabbitBusServiceBuilder WithBus(string busName)
    {
        return new RabbitBusServiceBuilder(this, busName).WithDefaultBus().WithDefaultProducer();
    }
    internal RabbitBusBuilder WithProducerContext()
    {
        return WithProducerContext<IRabbitProducerContext, RabbitProducerContext>();
    }
    public MainBusServiceBuilder BuildRabbit()
    {
        return Build();
    }
}