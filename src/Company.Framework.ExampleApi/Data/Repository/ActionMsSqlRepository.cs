
using Company.Framework.Data.Rdbms.Context;
using Company.Framework.Data.Rdbms.Repository;
using Company.Framework.ExampleApi.Data.Entity;

namespace Company.Framework.ExampleApi.Data.Repository
{
    public class ActionMsSqlRepository : CoreRdbmsRepository<ActionEntity, Guid>, IActionRepository
    {
        public ActionMsSqlRepository(IRdbmsDbContext dbContext) : base(dbContext)
        {
        }
    }

    public class FooMsSqlRepository : CoreRdbmsRepository<Foo, int>, IFooRepository
    {
        public FooMsSqlRepository(IRdbmsDbContext dbContext) : base(dbContext)
        {
        }
    }
}
