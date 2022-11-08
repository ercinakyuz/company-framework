using Company.Framework.Data.Mongo.Context;
using Company.Framework.Data.Mongo.Repository;
using Company.Framework.Data.Repository;
using Company.Framework.ExampleApi.Data.Entity;

namespace Company.Framework.ExampleApi.Data.Repository
{
    public class ActionRepository : CoreOptionalMongoRepository<ActionEntity, Guid>, IActionRepository
    {
        public ActionRepository(IMongoDbContext dbContext) : base(dbContext, "actions")
        {
        }
    }

    public interface IActionRepository : IOptionalRepository<ActionEntity, Guid>
    {
    }
}
