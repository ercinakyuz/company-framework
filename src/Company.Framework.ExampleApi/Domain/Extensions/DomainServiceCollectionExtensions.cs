using Company.Framework.Domain.Model.Aggregate.Event.Distribution.Extensions;
using Company.Framework.Domain.Model.Aggregate.OfWork.Extensions;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.Builder;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.Converter;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.OfWork;
using Company.Framework.Messaging.Envelope.Builder.Extensions;

namespace Company.Framework.ExampleApi.Domain.Extensions
{
    public static class DomainServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainComponents(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddEnvelope()
                .AddEventDistribution()
                .AddAggregations();
        }

        private static IServiceCollection AddAggregations(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddActionAggregation();
        }

        private static IServiceCollection AddActionAggregation(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IActionBuilder, ActionBuilder>()
                .AddAggregateOfWork<IActionOfWork, ActionOfWork>()
                .AddSingleton<IActionConverter, ActionConverter>();
        }
    }
}
