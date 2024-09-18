using Company.Framework.Core.Monad;
using Company.Framework.Core.Tenancy.Models;

namespace Company.Framework.Core.Tenancy.Components
{
    internal class TenantContext : ITenantBuilder, ITenantAccessor
    {
        private static readonly AsyncLocal<ITenant> TenantHolder;

        private readonly ITenantResolver _resolver;

        static TenantContext()
        {
            TenantHolder = new AsyncLocal<ITenant>();
        }

        public TenantContext(ITenantResolver resolver)
        {
            _resolver = resolver;
        }

        public Optional<ITenant> Get() => Optional<ITenant>.OfNullable(TenantHolder.Value);

        public void Build(TenantId? id)
        {
            var tenant = _resolver.Resolve(id).OrElseThrow(() => new System.Exception());
            TenantHolder.Value = tenant;
        }
    }
}
