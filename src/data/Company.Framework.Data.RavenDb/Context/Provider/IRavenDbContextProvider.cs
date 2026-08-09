using Company.Framework.Data.Db.Context.Provider;

namespace Company.Framework.Data.Raven.Context.Provider;

public interface IRavenDbContextProvider : IDbContextProvider<IRavenDbContext>
{
}
