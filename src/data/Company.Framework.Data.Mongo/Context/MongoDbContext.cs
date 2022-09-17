using MongoDB.Driver;

namespace Company.Framework.Data.Mongo.Context;

public class MongoDbContext : IMongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IMongoDatabase database)
    {
        _database = database;
    }

    public IMongoCollection<TEntity> GetCollection<TEntity>() => _database.GetCollection<TEntity>($"{typeof(TEntity).Name}");

    public IMongoCollection<TEntity> GetCollection<TEntity>(string name) => _database.GetCollection<TEntity>(name);
}