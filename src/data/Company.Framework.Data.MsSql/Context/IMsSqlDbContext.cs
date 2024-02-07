using Company.Framework.Data.Db.Context;
using Microsoft.EntityFrameworkCore;

namespace Company.Framework.Data.MySql.Context;

public interface IMsSqlDbContext : IDbContext
{
    DbSet<TEntity> Set<TEntity>() where TEntity : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    void Migrate();



}