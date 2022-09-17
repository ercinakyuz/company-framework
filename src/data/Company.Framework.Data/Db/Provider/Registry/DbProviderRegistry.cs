using System.Collections.Concurrent;
using Company.Framework.Data.Db.Context.Provider;
using Company.Framework.Data.Settings;

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
    }
}
