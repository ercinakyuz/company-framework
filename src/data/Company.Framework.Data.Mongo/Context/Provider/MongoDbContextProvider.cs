using System.Collections.Immutable;
using Company.Framework.Data.Db.Settings;
using MongoDB.Driver;

namespace Company.Framework.Data.Mongo.Context.Provider;

public class MongoDbContextProvider : IMongoDbContextProvider
{
    private readonly IReadOnlyDictionary<string, IMongoDbContext> _dbContextDictionary;

    public MongoDbContextProvider(DbProviderSettings settings)
    {
        var mongoClient = new MongoClient(settings.Connection.String);
        _dbContextDictionary = settings.Contexts.ToImmutableDictionary(
            context => context.Key, context => (IMongoDbContext)new MongoDbContext(mongoClient.GetDatabase(context.DbName)));
    }

    public IMongoDbContext Resolve(string key)
    {
        if (!_dbContextDictionary.TryGetValue(key, out var dbContext))
            throw new EntryPointNotFoundException($"Db context does not exist for key: {key}");
        return dbContext;
    }

    public IEnumerable<IMongoDbContext> ResolveAll()
    {
        return _dbContextDictionary.Values;
    }
}