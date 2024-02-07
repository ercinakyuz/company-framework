using Company.Framework.Data.Db.Provider.Registry;
using Company.Framework.Data.Db.Settings;
using Company.Framework.Data.Rdbms.Context;
using Company.Framework.Data.Rdbms.Context.Provider;
using Company.Framework.Data.Repository;
using Company.Framework.Data.Repository.Extensions;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Framework.Data.Rdbms.Extensions
{
    public static class MySqlDbServiceCollectionExtensions
    {
        public static IServiceCollection AddMySqlDb(this IServiceCollection serviceCollection)
        {
            DbProviderRegistry.Register(DbType.MySql, BuildContextProvider);
            return serviceCollection;
        }

        public static IServiceCollection AddMySqlRepository<TAbstraction, TImplementation>(this IServiceCollection serviceCollection, RepositorySettings settings)
        where TAbstraction : class, IRepository
            where TImplementation : class, TAbstraction
        {
            return serviceCollection.AddRepository<TAbstraction, TImplementation, IRdbmsDbContext>(settings);
        }

        private static IRdbmsDbContextProvider BuildContextProvider(DbProviderSettings settings)
        {
            return new RdbmsDbContextProvider(settings, MySQLConfiguration.Standard.ConnectionString(settings.Connection.String));
        }
    }
}
