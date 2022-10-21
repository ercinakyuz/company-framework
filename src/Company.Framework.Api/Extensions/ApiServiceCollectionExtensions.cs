using Company.Framework.Api.Handlers;
using Company.Framework.Api.Models.Error.Contract.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Framework.Api.Extensions
{
    public static class ApiServiceCollectionExtensions
    {
        public static IServiceCollection AddApiExceptionHandler(this IServiceCollection services)
        {
            return services
                .AddSingleton<ErrorContractBuilder>()
                .AddSingleton<ApiExceptionHandler>();
        }
    }
}
