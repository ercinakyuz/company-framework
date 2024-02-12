using Microsoft.EntityFrameworkCore;

namespace Company.Framework.Data.EntityFramework.Context
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
            return _dbContext.Set<TEntity>();
        }

    }
}
