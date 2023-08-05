using Company.Framework.Data.Mongo.Context;
using Company.Framework.Data.Mongo.Repository;
using Company.Framework.ExampleApi.Data.Entity;

namespace Company.Framework.ExampleApi.Data.Repository
{
    public class ActionRepository : CoreMongoRepository<ActionEntity, Guid>, IActionRepository
    {
        public ActionRepository(IMongoDbContext dbContext) : base(dbContext)
        {
        }
        public ActionRepository(IMongoDbContext dbContext, string collectionName) : base(dbContext, collectionName)
        {
        }
    }
}
