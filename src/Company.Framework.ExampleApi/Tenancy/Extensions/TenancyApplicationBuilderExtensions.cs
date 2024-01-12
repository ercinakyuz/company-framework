using Company.Framework.Core.Tenancy.Models;
using Company.Framework.Tenancy;

namespace Company.Framework.ExampleApi.Tenancy.Extensions
{
    public static class TenancyApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseMultiTenantApi(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.RegisterTenants().UseMiddleware<TenancyMiddleware>();
        }

        private static IApplicationBuilder RegisterTenants(this IApplicationBuilder applicationBuilder)
        {
            var tenantRegistrar = applicationBuilder.ApplicationServices.GetRequiredService<ITenantRegistrar>();
            tenantRegistrar.Register(TenantId.From(1), "Hemen");
            tenantRegistrar.Register(TenantId.From(2), "Sanalmarket");
            tenantRegistrar.Register(TenantId.From(3), "Ekstra");
            tenantRegistrar.Register(TenantId.From(4), "Yemek");
            return applicationBuilder;
        }
    }
}
