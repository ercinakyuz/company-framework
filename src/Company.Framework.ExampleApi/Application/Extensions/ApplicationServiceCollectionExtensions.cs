using Company.Framework.Application.Validation.Processors;
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
                .AddMediator().WithPreProcessor(typeof(CommandValidationProcessor<>)).Build();

        }
    }
}
