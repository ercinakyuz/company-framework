﻿using Company.Framework.Aspect.Extensions;
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
    }
}
