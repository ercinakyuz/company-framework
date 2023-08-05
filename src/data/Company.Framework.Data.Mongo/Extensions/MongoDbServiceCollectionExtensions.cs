using Company.Framework.Data.Db.Provider.Registry;
using Company.Framework.Data.Db.Settings;
using Company.Framework.Data.Mongo.Context;
using Company.Framework.Data.Mongo.Context.Provider;
using Company.Framework.Data.Repository;
using Company.Framework.Data.Repository.Extensions;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;

namespace Company.Framework.Data.Mongo.Extensions
{
    public static class MongoDbServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoDb(this IServiceCollection serviceCollection)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.CSharpLegacy));
            ConventionRegistry.Register("camelCase", new ConventionPack { new CamelCaseElementNameConvention() }, _ => true);
            DbProviderRegistry.Register(DbType.Mongo, settings => new MongoDbContextProvider(settings));
            return serviceCollection;
        }

        public static IServiceCollection AddMongoRepository<TAbstraction, TImplementation>(this IServiceCollection serviceCollection, RepositorySettings settings)
        where TAbstraction : class, IRepository
            where TImplementation : class, TAbstraction
        {
            return serviceCollection.AddRepository<TAbstraction, TImplementation, IMongoDbContext>(settings);
        }
    }
}
