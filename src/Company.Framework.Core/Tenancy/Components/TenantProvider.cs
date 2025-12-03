using Company.Framework.Core.Monad;
using Company.Framework.Core.Tenancy.Models;

namespace Company.Framework.Core.Tenancy.Components
{
    internal class TenantProvider : ITenantRegistrar, ITenantResolver
    {
        private readonly ISet<ITenant> _registeredTenants;

        public TenantProvider()
        {
            _registeredTenants = new HashSet<ITenant>();
        }

        public void Register(TenantId id, string name)
        {
            _registeredTenants.Add(new Tenant(id, name));
        }

        public Optional<ITenant> Resolve(TenantId? id)
        {
            return Optional<ITenant>.OfNullable(_registeredTenants.FirstOrDefault(t => t.Id == id));
        }

        public Optional<ITenant> Resolve(string name)
        {
            return Optional<ITenant>.OfNullable(_registeredTenants.FirstOrDefault(t => t.Name == name));
        }
    }
}
