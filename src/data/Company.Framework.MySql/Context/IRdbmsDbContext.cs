using Company.Framework.Data.Db.Context;
using NHibernate;

namespace Company.Framework.Data.Rdbms.Context;

public interface IRdbmsDbContext : IDbContext
{
    ISession Session { get; }
}