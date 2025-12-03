using Company.Framework.Aspect.Extensions;
using Company.Framework.Domain.Model.Aggregate.Event.Distribution.Processors;
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
                .ProxyServiceBuilder<TAbstraction>()
                .WithPostProcessor<EventDistributionProcessor>()
                .WithPostProcessor<BatchEventDistributionProcessor>()
                .Build<TImplementation>();
        }

        public static IServiceCollection AddSingleAggregateOfWork<TAbstraction, TImplementation>(this IServiceCollection serviceCollection)
            where TAbstraction : class, ISingleAggregateOfWork
            where TImplementation : class, TAbstraction
        {
            return serviceCollection
                .ProxyServiceBuilder<TAbstraction>()
                .WithPostProcessor<EventDistributionProcessor>()
                .Build<TImplementation>();
        }

        public static IServiceCollection AddBatchAggregateOfWork<TAbstraction, TImplementation>(this IServiceCollection serviceCollection)
            where TAbstraction : class, IBatchAggregateOfWork
            where TImplementation : class, TAbstraction
        {
            return serviceCollection
                .ProxyServiceBuilder<TAbstraction>()
                .WithPostProcessor<BatchEventDistributionProcessor>()
                .Build<TImplementation>();
        }
    }
}
