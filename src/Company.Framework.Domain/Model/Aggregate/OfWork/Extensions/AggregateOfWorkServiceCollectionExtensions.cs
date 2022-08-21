using Company.Framework.Aspect.Extensions;
using Company.Framework.Domain.Model.Aggregate.Event.Dispatcher.Processors;
using Company.Framework.Domain.Model.Aggregate.OfWork.Processors;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Framework.Domain.Model.Aggregate.OfWork.Extensions
{
    public static class AggregateOfWorkServiceCollectionExtensions
    {
        public static IServiceCollection AddAggregateOfWork<TAbstraction, TImplementation>(this IServiceCollection serviceCollection)
            where TAbstraction : class, IAggregateOfWork
            where TImplementation : class, TAbstraction
        {
            return serviceCollection
                .AddProcessors()
                .AddProxiedComponent<TAbstraction, TImplementation, AggregateOfWorkPreProcessor, AggregateOfWorkPostProcessor>();
        }

        private static IServiceCollection AddProcessors(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<AggregateOfWorkPostProcessor, EventDistributionProcessor>();
        }
    }
}
