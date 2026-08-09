using Company.Framework.Data.Db.Provider.Registry;
using Company.Framework.Data.Db.Settings;
using Company.Framework.Data.Raven.Context;
using Company.Framework.Data.Raven.Context.Provider;
using Company.Framework.Data.Repository;
using Company.Framework.Data.Repository.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Framework.Data.Raven.Extensions;

public static class RavenDbServiceCollectionExtensions
{
    public static IServiceCollection AddRavenDb(this IServiceCollection services)
    {
        DbProviderRegistry.Register(DbType.Raven, settings => new RavenDbContextProvider(settings));
        return services;
    }

    public static IServiceCollection AddRavenRepository<TAbstraction, TImplementation>(
        this IServiceCollection serviceCollection, 
        RepositorySettings settings)
        where TAbstraction : class, IRepository
        where TImplementation : class, TAbstraction
    {
        return serviceCollection.AddRepository<TAbstraction, TImplementation, IRavenDbContext>(settings);
    }
}