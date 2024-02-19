using Company.Framework.Data.Rdbms.Context;
using Company.Framework.Data.Repository;
using Company.Framework.Data.Repository.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Framework.Data.Rdbms.Extensions
{
    public static class RdbmsServiceCollectionExtensions
    {
        public static IServiceCollection AddRdbmsRepository<TAbstraction, TImplementation>(this IServiceCollection serviceCollection, RepositorySettings settings)
        where TAbstraction : class, IRepository
            where TImplementation : class, TAbstraction
        {
            return serviceCollection.AddRepository<TAbstraction, TImplementation, IRdbmsDbContext>(settings);
        }
    }
}
