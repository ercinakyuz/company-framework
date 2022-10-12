using System.Collections.Concurrent;
using Company.Framework.Data.Db.Context.Provider;
using Company.Framework.Data.Db.Settings;

namespace Company.Framework.Data.Db.Provider.Registry
{
    public class DbProviderRegistry
    {
        internal static readonly IDictionary<DbType, Func<DbProviderSettings, IDbContextProvider>> DbProviderDelegationRegistries
            = new ConcurrentDictionary<DbType, Func<DbProviderSettings, IDbContextProvider>>();

        public static void Register(DbType dbType, Func<DbProviderSettings, IDbContextProvider> dbContextProviderDelegate)
        {
            DbProviderDelegationRegistries.TryAdd(dbType, dbContextProviderDelegate);
        }

        internal static Func<DbProviderSettings, IDbContextProvider> Resolve(DbType dbType)
        {
            if (!DbProviderDelegationRegistries.TryGetValue(dbType, out var dbProviderDelegate))
                throw new InvalidOperationException($"No available database provider type for {dbType}");
            return dbProviderDelegate;
        }
    }
}
