using System.Collections.Concurrent;
using Company.Framework.Data.Settings;
using MongoDB.Driver;

namespace Company.Framework.Data.Mongo.Context.Provider;

public class MongoDbContextProvider : IMongoDbContextProvider
{
    private readonly IDictionary<string, IMongoDbContext> _dbContextDictionary;

    public MongoDbContextProvider(DbProviderSettings settings)
    {
        _dbContextDictionary = new ConcurrentDictionary<string, IMongoDbContext>();
        var mongoClient = new MongoClient(settings.Connection.String);
        Array.ForEach(settings.Contexts, context =>
        {
            _dbContextDictionary[context.Key] = new MongoDbContext(mongoClient.GetDatabase(context.DbName));
        });
    }

    public IMongoDbContext Resolve(string key)
    {
        if (!_dbContextDictionary.TryGetValue(key, out var dbContext))
            throw new EntryPointNotFoundException($"Db context does not exist for key: {key}");
        return dbContext;
    }
}