using Company.Framework.Messaging.Bus.Provider;
using Company.Framework.Messaging.HostedServices;
using Company.Framework.Messaging.Producer.Context;
using Company.Framework.Messaging.Producer.Context.Provider;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Company.Framework.Messaging.Bus.Builder;

public class MainBusServiceBuilder
{
    public IServiceCollection ServiceCollection { get; }

    private static bool _isBuilt;

    public MainBusServiceBuilder(IServiceCollection serviceCollection)
    {
        ServiceCollection = serviceCollection;
    }

    public IServiceCollection BuildBusServices()
    {
        if (_isBuilt)
            throw new InvalidOperationException("Bus services built before");
        _isBuilt = true;
        return ServiceCollection;
    }

    internal MainBusServiceBuilder WithBusProvider()
    {
        ServiceCollection.TryAddSingleton<IBusProvider, BusProvider>();
        return this;
    }
    internal MainBusServiceBuilder WithProducerContextProvider()
    {
        ServiceCollection
            .AddSingleton<IProducerContextProvider, ProducerContextProvider>()
            .AddSingleton<Func<Type, IProducerContext>>(serviceProvider => contextType => (IProducerContext)serviceProvider.GetRequiredService(contextType));
        return this;
    }

    internal MainBusServiceBuilder WithConsumersHostedService()
    {
        ServiceCollection.AddHostedService<ConsumersHostedService>();
        return this;
    }
}