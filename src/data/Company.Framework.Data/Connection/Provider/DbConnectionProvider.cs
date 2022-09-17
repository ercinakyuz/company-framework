using System.Collections.Concurrent;
using Company.Framework.Data.Context.Provider;
using Company.Framework.Data.Settings;
using Microsoft.Extensions.Options;

namespace Company.Framework.Data.Connection.Provider
{
    public class DbConnectionProvider : IDbConnectionProvider
    {
        private static readonly IDictionary<DbType, Func<DbProviderSettings, IDbContextProvider>> DbProviderDelegateDictionary 
            = new ConcurrentDictionary<DbType, Func<DbProviderSettings, IDbContextProvider>>();

        private readonly IDictionary<string, IDbContextProvider> _registeredDbProviderDictionary;

        public DbConnectionProvider(IOptions<DbSettings> options)
        {
            _registeredDbProviderDictionary = new Dictionary<string, IDbContextProvider>();
            Array.ForEach(options.Value.Instances, Register);
        }

        public static void AddDbContextProvider(DbType dbType, Func<DbProviderSettings, IDbContextProvider> dbContextProviderDelegate)
        {
            DbProviderDelegateDictionary.Add(dbType, dbContextProviderDelegate);
        }
        public void Register(DbInstanceSettings instanceSettings)
        {
            if (!DbProviderDelegateDictionary.TryGetValue(instanceSettings.Type, out var dbProviderDelegate))
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
