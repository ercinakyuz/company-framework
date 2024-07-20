using Company.Framework.Core.Id.Implementations;

namespace Company.Framework.Core.Tenancy.Models
{
    public record TenantId(int? Value) : IdOfInteger<TenantId>(Value)
    {
    }
}
