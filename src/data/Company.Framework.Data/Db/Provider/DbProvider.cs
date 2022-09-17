using Company.Framework.Data.Db.Context.Provider;
using Company.Framework.Data.Db.Provider.Registry;
using Company.Framework.Data.Db.Settings;
using Microsoft.Extensions.Options;

namespace Company.Framework.Data.Db.Provider
{
    public class DbProvider : IDbProvider
    {
        private readonly IDictionary<string, IDbContextProvider> _registeredDbProviderDictionary;

        public DbProvider(IOptions<DbSettings> options)
        {
            _registeredDbProviderDictionary = new Dictionary<string, IDbContextProvider>();
            Array.ForEach(options.Value.Instances, Register);
        }

        public void Register(DbInstanceSettings instanceSettings)
        {
            if (!DbProviderRegistry.DbProviderDelegationRegistries.TryGetValue(instanceSettings.Type, out var dbProviderDelegate))
                throw new InvalidOperationException($"No available database provider type for {instanceSettings.Type}");
            _registeredDbProviderDictionary[instanceSettings.Name] = dbProviderDelegate(instanceSettings.Provider);
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
