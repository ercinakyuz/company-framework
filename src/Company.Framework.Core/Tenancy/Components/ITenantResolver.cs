using Company.Framework.Core.Monad;
using Company.Framework.Core.Tenancy.Models;

namespace Company.Framework.Core.Tenancy.Components
{
    public interface ITenantResolver
    {
        Optional<ITenant> Resolve(TenantId? id);

        Optional<ITenant> Resolve(string name);
    }
}
