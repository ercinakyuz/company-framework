using Company.Framework.Messaging.Bus;
using Company.Framework.Messaging.Bus.Builder;
using Company.Framework.Messaging.RabbitMq.Connection.Context.Provider;
using Company.Framework.Messaging.RabbitMq.Producer.Context;
using Company.Framework.Messaging.RabbitMq.Producer.Provider;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Framework.Messaging.RabbitMq.Bus.Builder;

public class RabbitBusBuilder : CoreBusBuilder<RabbitBusBuilder>
{
    public RabbitBusBuilder(MainBusServiceBuilder mainBusServiceBuilder) : base(mainBusServiceBuilder)
    {
    }
    public RabbitBusServiceBuilder WithBus(string busName)
    {
        ServiceCollection.AddSingleton<IBus>(serviceProvider => ActivatorUtilities.CreateInstance<RabbitBus>(serviceProvider, busName));
        return new RabbitBusServiceBuilder(this, busName).WithConnectionContext().WithDefaultProducer();
    }

    internal RabbitBusBuilder WithProviders()
    {
        ServiceCollection
            .AddSingleton<IRabbitConnectionContextProvider, RabbitConnectionContextProvider>()
            .AddSingleton<IRabbitProducerContextProvider, RabbitProducerContextProvider>();
        return this;
    }
    public MainBusServiceBuilder BuildRabbit()
    {
        return Build();
    }
}