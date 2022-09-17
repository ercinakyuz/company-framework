using Company.Framework.Data.Context.Provider;

namespace Company.Framework.Data;

public interface IDbConnectionProvider
{
    IDbContextProvider Resolve(string key);

    TProvider Resolve<TProvider>(string key) where TProvider : IDbContextProvider;
}