namespace Company.Framework.Data.Context.Provider;

public interface IDbContextProvider<out TContext> : IDbContextProvider where TContext : IDbContext
{
    TContext Resolve(string key);
}

public interface IDbContextProvider
{
}
