using Company.Framework.Data.Db.Context;
using Company.Framework.Data.Db.Context.Provider;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Framework.Data.Repository.Extensions
{
    public static class RepositoryServiceCollectionExtensions
    {
        public static IServiceCollection AddRepository<TAbstraction, TImplementation, TContext>(this IServiceCollection serviceCollection, string instanceName, string contextKey)
            where TAbstraction : class, IRepository
            where TImplementation : class, TAbstraction
            where TContext : IDbContext
        {
            return serviceCollection
                .AddSingleton<TAbstraction, TImplementation>(provider =>
                {
                    var dbContext = provider.GetRequiredService<IDbProvider>().Resolve<IDbContextProvider<TContext>>(instanceName).Resolve(contextKey);
                    return ActivatorUtilities.CreateInstance<TImplementation>(provider, dbContext);
                });
        }
    }
}
