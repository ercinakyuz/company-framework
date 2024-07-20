using System;

namespace Company.Framework.Core.Tenancy.Models
{
    internal record Tenant(TenantId Id, string Name) : ITenant;
}
