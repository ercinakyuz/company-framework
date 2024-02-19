using Company.Framework.Data.Db.Provider.Registry;
using Company.Framework.Data.Db.Settings;
using Company.Framework.Data.Rdbms.Context.Provider;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Dialect;

namespace Company.Framework.Data.Rdbms.MsSql.Extensions
{
    public static class DbServiceCollectionExtensions
    {
        public static IServiceCollection AddMsSqlDb(this IServiceCollection serviceCollection)
        {
            DbProviderRegistry.Register(DbType.MsSql, BuildContextProvider);
            return serviceCollection;
        }

        private static IRdbmsDbContextProvider BuildContextProvider(DbProviderSettings settings)
        {
            return new RdbmsDbContextProvider<MsSql2012Dialect>(settings);
        }
    }
}
