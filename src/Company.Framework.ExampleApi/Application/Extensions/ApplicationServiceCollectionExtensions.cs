using Company.Framework.Mediator.Extensions;
using Company.Framework.Validation.Extensions;

namespace Company.Framework.ExampleApi.Application.Extensions
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationComponents(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddValidation()
                .AddMediator()
                //TODO: Remove when pre processors fully compatible with auto configuration
                //.WithCommandValidation()
                .Build();

        }
    }
}
