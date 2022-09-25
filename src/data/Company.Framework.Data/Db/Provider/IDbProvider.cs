using Company.Framework.Data.Db.Context.Provider;

namespace Company.Framework.Data.Db.Provider;

public interface IDbProvider
{
    TProvider Resolve<TProvider>(string key) where TProvider : IDbContextProvider;
}