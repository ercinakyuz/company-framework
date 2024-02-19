using Company.Framework.Data.Mongo.Context;
using Company.Framework.Data.Mongo.Repository;
using Company.Framework.ExampleApi.Data.Entity;

namespace Company.Framework.ExampleApi.Data.Repository
{
    public class ActionMongoRepository : CoreMongoRepository<ActionEntity, Guid>, IAction4Repository
    {
        public ActionMongoRepository(IMongoDbContext dbContext) : base(dbContext)
        {
        }
        public ActionMongoRepository(IMongoDbContext dbContext, string collectionName) : base(dbContext, collectionName)
        {
        }
    }
}
