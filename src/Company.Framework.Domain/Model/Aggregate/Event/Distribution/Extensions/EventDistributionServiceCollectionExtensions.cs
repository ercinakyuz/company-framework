using Company.Framework.Domain.Model.Aggregate.Event.Distribution.Handlers;
using Company.Framework.Domain.Model.Aggregate.Event.Distribution.Processors;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Framework.Domain.Model.Aggregate.Event.Distribution.Extensions;

public static class EventDistributionServiceCollectionExtensions
{

    public static IServiceCollection AddEventDistribution(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddSingleton<IEventDistributionHandler, EventDistributionHandler>()
            .AddSingleton<EventDistributionProcessor>()
            .AddSingleton<BatchEventDistributionProcessor>();
    }
}
