using Company.Framework.Core.Monad;
using Company.Framework.Core.Tenancy.Models;

namespace Company.Framework.Core.Tenancy.Components
{
    public interface ITenantAccessor
    {
        Optional<ITenant> Get();
    }
}
