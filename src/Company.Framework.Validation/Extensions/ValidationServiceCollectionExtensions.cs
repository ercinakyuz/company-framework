using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Framework.Validation.Extensions
{
    public static class ValidationServiceCollectionExtensions
    {
        public static IServiceCollection AddValidation(this IServiceCollection services)
        {
            return services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
