using Company.Framework.Data.Db.Provider.Registry;
using Company.Framework.Data.Db.Settings;
using Company.Framework.Data.MsSql.Context;
using Company.Framework.Data.MsSql.Context.Provider;
using Company.Framework.Data.Repository;
using Company.Framework.Data.Repository.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Framework.Data.MsSql.Extensions
{
    public static class MsSqlDbServiceCollectionExtensions
    {
        public static IServiceCollection AddMsSqlDb(this IServiceCollection serviceCollection)
        {
            DbProviderRegistry.Register(DbType.MsSql, settings => new MsSqlDbContextProvider(settings));
            return serviceCollection;
        }

        public static IServiceCollection AddMsSqlRepository<TAbstraction, TImplementation>(this IServiceCollection serviceCollection, RepositorySettings settings)
        where TAbstraction : class, IRepository
            where TImplementation : class, TAbstraction
        {
            return serviceCollection.AddRepository<TAbstraction, TImplementation, IMsSqlDbContext>(settings);
        }
    }
}
