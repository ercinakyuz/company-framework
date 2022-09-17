using Company.Framework.Data.Db.Context.Provider;

namespace Company.Framework.Data;

public interface IDbProvider
{
    IDbContextProvider Resolve(string key);

    TProvider Resolve<TProvider>(string key) where TProvider : IDbContextProvider;
}