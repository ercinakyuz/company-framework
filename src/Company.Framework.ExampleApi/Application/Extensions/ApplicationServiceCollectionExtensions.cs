using Company.Framework.Mediator.Extensions;

namespace Company.Framework.ExampleApi.Application.Extensions
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationComponents(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddMediator();
        }
    }
}
