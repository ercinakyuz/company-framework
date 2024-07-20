using Company.Framework.Core.Tenancy.Models;

namespace Company.Framework.Core.Tenancy.Components
{
    public interface ITenantBuilder
    {
        void Build(TenantId? id);
    }
}
