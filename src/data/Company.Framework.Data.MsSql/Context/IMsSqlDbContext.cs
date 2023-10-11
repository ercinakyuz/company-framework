using Company.Framework.Data.Db.Context;
using Microsoft.EntityFrameworkCore;

namespace Company.Framework.Data.MsSql.Context;

public interface IMsSqlDbContext : IDbContext
{
    DbSet<TEntity> GetDbSet<TEntity>() where TEntity : class;

    DbSet<TEntity> GetDbSet<TEntity>(string name) where TEntity : class;

}