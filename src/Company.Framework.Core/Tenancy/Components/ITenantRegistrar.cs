using Company.Framework.Core.Tenancy.Models;

namespace Company.Framework.Core.Tenancy.Components
{
    public interface ITenantRegistrar
    {
        void Register(TenantId id, string name);
    }
}
