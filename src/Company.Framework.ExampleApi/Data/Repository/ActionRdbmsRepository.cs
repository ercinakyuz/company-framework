using Company.Framework.Data.Rdbms.Context;
using Company.Framework.Data.Rdbms.Repository;
using Company.Framework.ExampleApi.Data.Entity;

namespace Company.Framework.ExampleApi.Data.Repository
{
    public class ActionRdbmsRepository : CoreRdbmsRepository<ActionEntity, Guid>, IActionRepository, IAction2Repository, IAction3Repository
    {
        public ActionRdbmsRepository(IRdbmsDbContext dbContext) : base(dbContext)
        {
        }
    }

    public class FooRdbmsRepository : CoreRdbmsRepository<Foo, int>, IFooRepository
    {
        public FooRdbmsRepository(IRdbmsDbContext dbContext) : base(dbContext)
        {
        }
    }
}
