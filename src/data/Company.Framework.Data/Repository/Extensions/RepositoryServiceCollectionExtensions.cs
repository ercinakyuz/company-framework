using Company.Framework.Data.Db.Context;
using Company.Framework.Data.Db.Context.Provider;
using Company.Framework.Data.Db.Provider;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Framework.Data.Repository.Extensions
{
    public static class RepositoryServiceCollectionExtensions
    {
        public static IServiceCollection AddRepository<TAbstraction, TImplementation, TContext>(this IServiceCollection serviceCollection, RepositorySettings settings)
            where TAbstraction : class, IRepository
            where TImplementation : class, TAbstraction
            where TContext : IDbContext
        {
            return serviceCollection
                .AddSingleton<TAbstraction, TImplementation>(serviceProvider =>
                {
                    var dbContext = serviceProvider.GetRequiredService<IDbProvider>().Resolve<IDbContextProvider<TContext>>(settings.Instance).Resolve(settings.Context);
                    return settings.TableOrCollection is null ? ActivatorUtilities.CreateInstance<TImplementation>(serviceProvider, dbContext) : ActivatorUtilities.CreateInstance<TImplementation>(serviceProvider, dbContext, settings.TableOrCollection);
                });
        }
    }
}
