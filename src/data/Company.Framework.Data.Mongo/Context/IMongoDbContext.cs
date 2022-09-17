using Company.Framework.Data.Context;
using MongoDB.Driver;

namespace Company.Framework.Data.Mongo.Context;

public interface IMongoDbContext: IDbContext
{
    public IMongoCollection<TEntity> GetCollection<TEntity>();
    public IMongoCollection<TEntity> GetCollection<TEntity>(string name);
}