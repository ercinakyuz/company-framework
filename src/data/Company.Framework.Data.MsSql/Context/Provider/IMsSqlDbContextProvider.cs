using Company.Framework.Data.Db.Context.Provider;

namespace Company.Framework.Data.MySql.Context.Provider
{
    public interface IMsSqlDbContextProvider : IDbContextProvider<IMsSqlDbContext>
    {
    }
}
