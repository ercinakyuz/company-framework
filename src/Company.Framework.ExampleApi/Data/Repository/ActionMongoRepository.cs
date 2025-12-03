using Company.Framework.Data.Mongo.Context;
using Company.Framework.Data.Mongo.Repository;
using Company.Framework.ExampleApi.Data.Entity;
using NUlid;

namespace Company.Framework.ExampleApi.Data.Repository
{
    public class ActionMongoRepository : CoreMongoRepository<ActionEntity, Ulid>, IActionRepository
    {
        public ActionMongoRepository(IMongoDbContext dbContext) : base(dbContext)
        {
        }
        public ActionMongoRepository(IMongoDbContext dbContext, string collectionName) : base(dbContext, collectionName)
        {
        }
    }
}
