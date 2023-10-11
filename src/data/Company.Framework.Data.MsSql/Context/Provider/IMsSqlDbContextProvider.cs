using Company.Framework.Data.Db.Context.Provider;

namespace Company.Framework.Data.MsSql.Context.Provider
{
    public interface IMsSqlDbContextProvider : IDbContextProvider<IMsSqlDbContext>
    {
    }
}
