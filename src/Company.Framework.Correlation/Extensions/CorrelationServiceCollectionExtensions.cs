using CorrelationId.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Framework.Correlation.Extensions
{
    public static class CorrelationServiceCollectionExtensions
    {
        public static IServiceCollection AddCorrelation(this IServiceCollection services)
        {
            return services.AddDefaultCorrelationId(options =>
            {
                options.AddToLoggingScope = true;
                options.LoggingScopeKey = "CorrelationId";
                options.RequestHeader = "correlation-id";
                options.ResponseHeader = "correlation-id";
            });
        }
    }
}
