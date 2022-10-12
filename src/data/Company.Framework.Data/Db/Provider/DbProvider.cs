using System.Collections.Immutable;
using Company.Framework.Data.Db.Context.Provider;
using Company.Framework.Data.Db.Provider.Registry;
using Company.Framework.Data.Db.Settings;
using Microsoft.Extensions.Options;

namespace Company.Framework.Data.Db.Provider
{
    public class DbProvider : IDbProvider
    {
        private readonly IReadOnlyDictionary<string, IDbContextProvider> _registeredDbProviderDictionary;

        public DbProvider(IOptions<DbSettings> options)
        {
            _registeredDbProviderDictionary= options.Value.Instances.ToImmutableDictionary(instance => instance.Name,
                instance => DbProviderRegistry.Resolve(instance.Type).Invoke(instance.Provider));
        }

        public TProvider Resolve<TProvider>(string key) where TProvider : IDbContextProvider
        {
            return (TProvider)Resolve(key);
        }

        public IDbContextProvider Resolve(string key)
        {
            if (!_registeredDbProviderDictionary.TryGetValue(key, out var dbProvider))
                throw new InvalidOperationException($"Database provider does not exist for given key: {key}");
            return dbProvider;
        }
    }
}
