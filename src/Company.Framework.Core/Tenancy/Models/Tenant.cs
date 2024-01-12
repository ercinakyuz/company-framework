using Company.Framework.Core.Id.Implementations;
using Company.Framework.Core.Monad;
using System;

namespace Company.Framework.Core.Tenancy.Models
{
    internal record Tenant(TenantId Id, string Name) : ITenant
    {
    }

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
            if (id is null)
                throw new System.Exception();
            return Optional<ITenant>.OfNullable(_registeredTenants.FirstOrDefault(t => t.Id == id));
        }

        public Optional<ITenant> Resolve(string name)
        {
            return Optional<ITenant>.OfNullable(_registeredTenants.FirstOrDefault(t => t.Name == name));
        }
    }

    public interface ITenantRegistrar
    {
        void Register(TenantId id, string name);
    }

    public interface ITenantResolver
    {
        Optional<ITenant> Resolve(TenantId? id);

        Optional<ITenant> Resolve(string name);
    }

    public interface ITenant
    {
        TenantId Id { get; }

        string Name { get; }
    }

    public record TenantId : IdOfInteger<TenantId>
    {
        public TenantId(int Value) : base(Value)
        {
        }
    }

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

    public interface ITenantAccessor
    {
        Optional<ITenant> Get();
    }

    public interface ITenantBuilder
    {
        void Build(TenantId? id);
    }
}
