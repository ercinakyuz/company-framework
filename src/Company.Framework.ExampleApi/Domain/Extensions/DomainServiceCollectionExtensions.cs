using Company.Framework.Domain.Model.Aggregate.OfWork.Extensions;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.Converter;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.OfWork;

namespace Company.Framework.ExampleApi.Domain.Extensions
{
    public static class DomainServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainComponents(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddAggregateOfWork<IActionOfWork, ActionOfWork>()
                .AddSingleton<ActionConverter>();
        }
    }
}
