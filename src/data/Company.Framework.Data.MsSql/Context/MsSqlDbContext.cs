using Microsoft.EntityFrameworkCore;

namespace Company.Framework.Data.MsSql.Context
{
    public class MsSqlDbContext : IMsSqlDbContext
    {
        private readonly DbContext _dbContext;

        public MsSqlDbContext(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DbSet<TEntity> GetDbSet<TEntity>() where TEntity : class
        {
            return GetDbSet<TEntity>($"{typeof(TEntity).Name}");
        }

        public DbSet<TEntity> GetDbSet<TEntity>(string name) where TEntity : class
        {
            return _dbContext.Set<TEntity>(name);
        }

    }
}
