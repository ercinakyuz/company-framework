using Company.Framework.Data.Connection.Provider;
using Company.Framework.Data.Extensions;
using Company.Framework.Data.Mongo.Context;
using Company.Framework.Data.Mongo.Context.Provider;
using Company.Framework.Data.Repository;
using Company.Framework.Data.Settings;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.Conventions;

namespace Company.Framework.Data.Mongo.Extensions
{
    public static class MongoDbServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoDb(this IServiceCollection serviceCollection)
        {
            var conventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("camelCase", conventionPack, _ => true);
            DbConnectionProvider.AddDbContextProvider(DbType.Mongo, dbContextSettings => new MongoDbContextProvider(dbContextSettings));
            return serviceCollection;
        }

        public static IServiceCollection AddMongoRepository<TAbstraction, TImplementation>(this IServiceCollection serviceCollection, string instanceName, string contextKey)
        where TAbstraction : class, IRepository
            where TImplementation : class, TAbstraction
        {
            return serviceCollection.AddRepository<TAbstraction, TImplementation, IMongoDbContext>(instanceName, contextKey);
        }
    }
}
