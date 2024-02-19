using Company.Framework.Data.Db.Context.Provider;

namespace Company.Framework.Data.EntityFramework.Context.Provider
{
    public interface IMsSqlDbContextProvider : IDbContextProvider<IMsSqlDbContext>
    {
    }
}
