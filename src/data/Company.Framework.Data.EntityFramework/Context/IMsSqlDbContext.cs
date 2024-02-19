using Company.Framework.Data.Db.Context;
using Microsoft.EntityFrameworkCore;

namespace Company.Framework.Data.EntityFramework.Context;

public interface IMsSqlDbContext : IDbContext
{
    DbSet<TEntity> Set<TEntity>() where TEntity : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    void Migrate();



}