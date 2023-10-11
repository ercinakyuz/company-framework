using System.Collections.Generic;
using System.Linq.Expressions;
using Company.Framework.Core.Linq.Extensions;
using Company.Framework.Core.Monad;
using Company.Framework.Data.Entity;
using Company.Framework.Data.MsSql.Context;
using Company.Framework.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace Company.Framework.Data.MsSql.Repository
{
    public abstract class CoreMsSqlRepository<TEntity, TId> : IRepository<TEntity, TId> where TEntity : CoreEntity<TId> where TId : struct
    {
        protected readonly DbSet<TEntity> DbSet;
        protected CoreMsSqlRepository(IMsSqlDbContext dbContext)
        {
            DbSet = dbContext.GetDbSet<TEntity>();
        }
        protected CoreMsSqlRepository(IMsSqlDbContext dbContext, string dbSetName)
        {
            DbSet =  dbContext.GetDbSet<TEntity>(dbSetName);
        }

        public virtual async IAsyncEnumerable<TEntity> FindAllAsync(Expression<Func<TEntity, bool>>? predicate = default, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = default)
        {
            IQueryable<TEntity> queryable = DbSet;
            if (predicate is not null)
            {
                queryable = queryable.Where(predicate);
            }

            await foreach (var entity in queryable.ToAsyncEnumerable()) yield return entity;
        }

        public virtual async Task<Optional<TEntity>> FindAsync(TId id)
        {
            return Optional<TEntity>.OfNullable(await FindUnsafeAsync(id).ConfigureAwait(false));
        }
        public virtual async Task InsertAsync(TEntity entity)
        {
            await DbSet.AddAsync(entity).ConfigureAwait(false);
        }
        public virtual async Task InsertManyAsync(IEnumerable<TEntity> entities)
        {
            await DbSet.AddRangeAsync(entities).ConfigureAwait(false);
        }

        public virtual Task UpdateAsync(TEntity entity)
        {
            DbSet.Update(entity);
            return Task.CompletedTask;
        }

        public virtual Task UpdateManyAsync(IEnumerable<TEntity> entities)
        {
            DbSet.UpdateRange(entities);
            return Task.CompletedTask;
        }

        public virtual Task DeleteAsync(TId id)
        {
            DbSet.Remove(DbSet.Single(entity => entity.Id.Equals(id)));
            return Task.CompletedTask;
        }

        public virtual Task DeleteManyAsync(Expression<Func<TEntity, bool>> filter)
        {
            DbSet.RemoveRange(DbSet.Where(filter));
            return Task.CompletedTask;
        }

        private async Task<TEntity?> FindUnsafeAsync(TId id)
        {
            return await DbSet.FindAsync(id).ConfigureAwait(false);
        }
    }
}
