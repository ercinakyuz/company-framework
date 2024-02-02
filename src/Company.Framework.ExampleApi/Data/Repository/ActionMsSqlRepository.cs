using Company.Framework.Data.MsSql.Context;
using Company.Framework.Data.MsSql.Repository;
using Company.Framework.ExampleApi.Data.Entity;

namespace Company.Framework.ExampleApi.Data.Repository
{
    public class ActionMsSqlRepository : CoreMsSqlRepository<ActionEntity, Guid>, IActionRepository
    {
        public ActionMsSqlRepository(IMsSqlDbContext dbContext) : base(dbContext)
        {
        }
    }
}
