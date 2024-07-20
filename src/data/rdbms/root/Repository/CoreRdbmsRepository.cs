using System.Collections.Immutable;
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

        private static readonly Action<ISession, TEntity> InsertAction = (session, entity) => session.Save(entity);

        private static readonly Action<ISession, TEntity> UpdateAction = (session, entity) => session.Update(entity);

        protected CoreRdbmsRepository(IRdbmsDbContext context)
        {
            Context = context;
        }

        public virtual IAsyncEnumerable<TEntity> FindAllAsync(Expression<Func<TEntity, bool>>? predicate = default, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = default)
        {
            var queryable = predicate is null ? Queryable : Queryable.Where(predicate);
            return queryable.ToAsyncEnumerable();
        }

        public virtual async Task<Optional<TEntity>> FindAsync(TId id)
        {
            return Optional<TEntity>.OfNullable(await FindUnsafeAsync(id).ConfigureAwait(false));
        }
        public virtual async Task InsertAsync(TEntity entity)
        {
            await InsertOrUpdateManyAsync(InsertAction, ImmutableList.Create(entity));
        }
        public virtual async Task InsertManyAsync(IEnumerable<TEntity> entities)
        {
            await InsertOrUpdateManyAsync(InsertAction, entities);
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            await InsertOrUpdateManyAsync(UpdateAction, ImmutableList.Create(entity));
        }

        public virtual async Task UpdateManyAsync(IEnumerable<TEntity> entities)
        {
            await InsertOrUpdateManyAsync(UpdateAction, entities);
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

        private async Task InsertOrUpdateManyAsync(Action<ISession, TEntity> action, IEnumerable<TEntity> entities)
        {
            using (var session = Session)
            {
                using (var transaction = session.BeginTransaction())
                {
                    foreach (var entity in entities)
                    {
                        action(session, entity);
                    }
                    await transaction.CommitAsync().ConfigureAwait(false);
                }
            }
        }
    }


}
