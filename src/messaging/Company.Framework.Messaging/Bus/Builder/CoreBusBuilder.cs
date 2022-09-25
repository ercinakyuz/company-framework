using Company.Framework.Messaging.Producer.Context;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Framework.Messaging.Bus.Builder;

public abstract class CoreBusBuilder<TBuilder> : IBusBuilder where TBuilder : CoreBusBuilder<TBuilder>
{
    private bool _isBuilt;
    protected readonly MainBusServiceBuilder MainBusServiceBuilder;

    public IServiceCollection ServiceCollection { get; }

    protected CoreBusBuilder(MainBusServiceBuilder mainBusServiceBuilder)
    {
        MainBusServiceBuilder = mainBusServiceBuilder;
        ServiceCollection = mainBusServiceBuilder.ServiceCollection;
    }

    protected MainBusServiceBuilder Build()
    {
        if (_isBuilt)
            throw new InvalidOperationException("Bus services built before");
        _isBuilt = true;
        return MainBusServiceBuilder;
    }

    protected TBuilder WithProducerContext<TAbstraction, TImplementation>()
        where TAbstraction : class, IProducerContext
        where TImplementation : class, TAbstraction
    {
        ServiceCollection
            .AddSingleton<TAbstraction, TImplementation>();
        return (TBuilder)this;
    }

}