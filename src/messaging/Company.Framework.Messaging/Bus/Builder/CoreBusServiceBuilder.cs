using Microsoft.Extensions.DependencyInjection;

namespace Company.Framework.Messaging.Bus.Builder;

public abstract class CoreBusServiceBuilder<TBusBuilder> where TBusBuilder : IBusBuilder
{
    private bool _isBuilt;
    protected readonly string BusName;
    protected readonly TBusBuilder BusBuilder;
    protected readonly IServiceCollection ServiceCollection;

    protected CoreBusServiceBuilder(TBusBuilder busBuilder, string busName)
    {
        BusBuilder = busBuilder;
        BusName = busName;
        ServiceCollection = BusBuilder.ServiceCollection;
    }

    public TBusBuilder BuildBus()
    {
        if (_isBuilt)
            throw new InvalidOperationException("Bus services built before");
        _isBuilt = true;
        return BusBuilder;
    }
}