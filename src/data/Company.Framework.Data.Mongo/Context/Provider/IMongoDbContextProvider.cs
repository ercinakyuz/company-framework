using Company.Framework.Data.Db.Context.Provider;

namespace Company.Framework.Data.Mongo.Context.Provider
{
    public interface IMongoDbContextProvider : IDbContextProvider<IMongoDbContext>
    {
    }
}
