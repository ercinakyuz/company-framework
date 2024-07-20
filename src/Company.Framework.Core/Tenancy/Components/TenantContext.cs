using Company.Framework.Core.Monad;
using Company.Framework.Core.Tenancy.Models;

namespace Company.Framework.Core.Tenancy.Components
{
    internal class TenantContext : ITenantBuilder, ITenantAccessor
    {
        private readonly ITenantResolver _resolver;

        private ITenant? _tenant;

        public TenantContext(ITenantResolver resolver)
        {
            _resolver = resolver;
        }

        public Optional<ITenant> Get() => Optional<ITenant>.OfNullable(_tenant);

        public void Build(TenantId? id)
        {
            _tenant = _resolver.Resolve(id).OrElseThrow(() => new System.Exception());
        }
    }
}
