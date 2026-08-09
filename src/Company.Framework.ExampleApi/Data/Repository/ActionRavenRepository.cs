using Company.Framework.Data.Raven.Context;
using Company.Framework.Data.Raven.Repository;
using Company.Framework.ExampleApi.Data.Entity;
using NUlid;

namespace Company.Framework.ExampleApi.Data.Repository;

public class ActionRavenRepository : CoreRavenRepository<ActionEntity, Ulid>, IActionRepository
{
    public ActionRavenRepository(IRavenDbContext dbContext) : base(dbContext)
    {
    }
}
