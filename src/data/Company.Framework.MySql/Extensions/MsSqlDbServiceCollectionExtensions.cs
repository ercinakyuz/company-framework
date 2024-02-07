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
    public static class MsSqlDbServiceCollectionExtensions
    {
        public static IServiceCollection AddMsSqlDb(this IServiceCollection serviceCollection)
        {
            DbProviderRegistry.Register(DbType.MsSql, BuildContextProvider);
            return serviceCollection;
        }

        public static IServiceCollection AddMsSqlRepository<TAbstraction, TImplementation>(this IServiceCollection serviceCollection, RepositorySettings settings)
        where TAbstraction : class, IRepository
            where TImplementation : class, TAbstraction
        {
            return serviceCollection.AddRepository<TAbstraction, TImplementation, IRdbmsDbContext>(settings);
        }

        private static IRdbmsDbContextProvider BuildContextProvider(DbProviderSettings settings)
        {
            return new RdbmsDbContextProvider(settings, MsSqlConfiguration.MsSql2012.ConnectionString(settings.Connection.String));
        }
    }
}
