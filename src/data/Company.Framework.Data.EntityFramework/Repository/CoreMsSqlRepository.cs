using System.Linq.Expressions;
using Company.Framework.Core.Linq.Extensions;
using Company.Framework.Core.Monad;
using Company.Framework.Data.Entity;
using Company.Framework.Data.EntityFramework.Context;
using Company.Framework.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace Company.Framework.Data.EntityFramework.Repository
{
    public abstract class CoreMsSqlRepository<TEntity, TId> : IRepository<TEntity, TId> where TEntity : CoreEntity<TId> where TId : struct
    {
        protected readonly DbSet<TEntity> Set;
        protected readonly IMsSqlDbContext Context;
        protected CoreMsSqlRepository(IMsSqlDbContext context)
        {
            Context = context;
            Set = context.Set<TEntity>();
        }

        public virtual IAsyncEnumerable<TEntity> FindAllAsync(Expression<Func<TEntity, bool>>? predicate = default, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = default)
        {
            IQueryable<TEntity> queryable = Set;
            if (predicate is not null)
            {
                queryable = queryable.Where(predicate);
            }

            return queryable.AsAsyncEnumerable();
        }

        public virtual async Task<Optional<TEntity>> FindAsync(TId id)
        {
            return Optional<TEntity>.OfNullable(await FindUnsafeAsync(id).ConfigureAwait(false));
        }
        public virtual async Task InsertAsync(TEntity entity)
        {
            await Set.AddAsync(entity).ConfigureAwait(false);
            await Context.SaveChangesAsync();
        }
        public virtual async Task InsertManyAsync(IEnumerable<TEntity> entities)
        {
            await Set.AddRangeAsync(entities).ConfigureAwait(false);
            await Context.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            Set.Update(entity);
            await Context.SaveChangesAsync();
        }

        public virtual async Task UpdateManyAsync(IEnumerable<TEntity> entities)
        {
            Set.UpdateRange(entities);
            await Context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(TId id)
        {
            Set.Remove(Set.Single(entity => entity.Id.Equals(id)));
            await Context.SaveChangesAsync();
        }

        public virtual async Task DeleteManyAsync(Expression<Func<TEntity, bool>> filter)
        {
            Set.RemoveRange(Set.Where(filter));
            await Context.SaveChangesAsync();
        }

        private async Task<TEntity?> FindUnsafeAsync(TId id)
        {
            return await Set.FindAsync(id).ConfigureAwait(false);
        }
    }
}
