using Company.Framework.Data.Context.Provider;

namespace Company.Framework.Data.Context;

public interface IDbContext
{
}

public interface IDbContextFactory
{
    IDbContext Create(IDbContextProvider dbContextProvider);
}