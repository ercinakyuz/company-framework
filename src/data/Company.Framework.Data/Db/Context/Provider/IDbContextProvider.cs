namespace Company.Framework.Data.Db.Context.Provider;

public interface IDbContextProvider<out TContext> : IDbContextProvider where TContext : IDbContext
{
    TContext Resolve(string key);
}

public interface IDbContextProvider
{
}
