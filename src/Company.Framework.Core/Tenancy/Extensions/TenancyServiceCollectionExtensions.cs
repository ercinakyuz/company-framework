using Company.Framework.Core.Tenancy.Components;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Framework.Core.Tenancy.Extensions
{
    public static class TenancyServiceCollectionExtensions
    {
        public static IServiceCollection AddTenancy(this IServiceCollection services)
        {
            return services.AddProvider().AddContext();
        }

        private static IServiceCollection AddProvider(this IServiceCollection services)
        {
            return services
                .AddSingleton<TenantProvider>()
                .AddSingleton<ITenantRegistrar>(serviceProvider => serviceProvider.GetRequiredService<TenantProvider>())
                .AddSingleton<ITenantResolver>(serviceProvider => serviceProvider.GetRequiredService<TenantProvider>());
        }

        private static IServiceCollection AddContext(this IServiceCollection services)
        {
            return services
                .AddScoped<TenantContext>()
                .AddScoped<ITenantBuilder>(serviceProvider => serviceProvider.GetRequiredService<TenantContext>())
                .AddScoped<ITenantAccessor>(serviceProvider => serviceProvider.GetRequiredService<TenantContext>());
        }
    }
}
