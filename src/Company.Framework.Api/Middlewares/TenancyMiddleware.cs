using Company.Framework.Core.Tenancy.Models;
using Microsoft.AspNetCore.Http;

namespace Company.Framework.Tenancy
{
    public class TenancyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITenantBuilder _tenantBuilder;

        public TenancyMiddleware(RequestDelegate next, ITenantBuilder tenantBuilder)
        {
            _next = next;
            _tenantBuilder = tenantBuilder;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var tenantIdFromHeader = context.Request.Headers["tenant-id"];
            if (string.IsNullOrWhiteSpace(tenantIdFromHeader) || !int.TryParse(tenantIdFromHeader, out var tenantIdValue))
                throw TenantNotAvailable();
            var tenantId = TenantId.From(tenantIdValue);
            _tenantBuilder.Build(tenantId);
            await _next(context);
        }

        private Exception TenantNotAvailable()
        {
            return new Exception("Tenant not available");
        }
    }
}
