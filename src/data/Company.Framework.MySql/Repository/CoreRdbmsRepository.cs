using System.Linq.Expressions;
using Company.Framework.Core.Linq.Extensions;
using Company.Framework.Core.Monad;
using Company.Framework.Data.Entity;
using Company.Framework.Data.Rdbms.Context;
using Company.Framework.Data.Repository;
using NHibernate;

namespace Company.Framework.Data.Rdbms.Repository
{
    public abstract class CoreRdbmsRepository<TEntity, TId> : IRepository<TEntity, TId> where TEntity : CoreEntity<TId>
    {
        protected readonly IRdbmsDbContext Context;

        protected ISession Session => Context.Session;

        protected IQueryable<TEntity> Queryable => Session.Query<TEntity>();

        protected CoreRdbmsRepository(IRdbmsDbContext context)
        {
            Context = context;
        }

        public virtual async IAsyncEnumerable<TEntity> FindAllAsync(Expression<Func<TEntity, bool>>? predicate = default, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = default)
        {
            var queryable = predicate is null ? Queryable : Queryable.Where(predicate);
            await foreach (var entity in queryable.ToAsyncEnumerable()) yield return entity;
        }

        public virtual async Task<Optional<TEntity>> FindAsync(TId id)
        {
            return Optional<TEntity>.OfNullable(await FindUnsafeAsync(id).ConfigureAwait(false));
        }
        public virtual async Task InsertAsync(TEntity entity)
        {
            await Session.SaveOrUpdateAsync(entity);
            await Session.FlushAsync();
        }
        public virtual async Task InsertManyAsync(IEnumerable<TEntity> entities)
        {
            await Session.SaveAsync(entities);
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            await Session.UpdateAsync(entity);
        }

        public virtual async Task UpdateManyAsync(IEnumerable<TEntity> entities)
        {
            await Session.UpdateAsync(entities);
        }

        public virtual async Task DeleteAsync(TId id)
        {
            await Session.DeleteAsync(Queryable.Single(q => q.Id.Equals(id)));
        }

        public virtual async Task DeleteManyAsync(Expression<Func<TEntity, bool>> filter)
        {
            await Session.DeleteAsync(Queryable.Where(filter));
        }

        private async Task<TEntity?> FindUnsafeAsync(TId id)
        {
            return await Session.GetAsync<TEntity>(id).ConfigureAwait(false);
        }
    }
}
