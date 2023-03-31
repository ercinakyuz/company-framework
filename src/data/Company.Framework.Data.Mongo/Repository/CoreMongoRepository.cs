using System.Linq.Expressions;
using Company.Framework.Data.Entity;
using Company.Framework.Data.Mongo.Context;
using Company.Framework.Data.Mongo.Extensions;
using Company.Framework.Data.Repository;
using MongoDB.Driver;

namespace Company.Framework.Data.Mongo.Repository
{
    public abstract class CoreMongoRepository<TEntity, TId> : IRepository<TEntity, TId> where TEntity : CoreEntity<TId> where TId : struct
    {
        protected readonly IMongoCollection<TEntity> Collection;
        protected CoreMongoRepository(IMongoDbContext dbContext)
        {
            Collection = dbContext.GetCollection<TEntity>();
        }
        protected CoreMongoRepository(IMongoDbContext dbContext, string collectionName)
        {
            Collection = dbContext.GetCollection<TEntity>(collectionName);
        }

        public virtual async IAsyncEnumerable<TEntity> FindAllAsync(Expression<Func<TEntity, bool>>? predicate = default, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = default)
        {
            await foreach (var entity in (await Collection.FindAsync(predicate).ConfigureAwait(false)).ToAsyncEnumerable().ConfigureAwait(false)) yield return entity;
        }

        public virtual async Task<Core.Monad.Optional<TEntity>> FindAsync(TId id)
        {
            return Core.Monad.Optional<TEntity>.OfNullable(await FindUnsafeAsync(id).ConfigureAwait(false));
        }
        public virtual async Task InsertAsync(TEntity entity)
        {
            await Collection.InsertOneAsync(entity).ConfigureAwait(false);
        }
        public virtual async Task InsertManyAsync(IEnumerable<TEntity> entities)
        {
            await Collection.InsertManyAsync(entities).ConfigureAwait(false);
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            await Collection.ReplaceOneAsync(e => e.Id.Equals(entity.Id), entity);
        }

        public virtual async Task DeleteManyAsync(Expression<Func<TEntity, bool>> filter)
        {
            await Collection.DeleteManyAsync(filter).ConfigureAwait(false);
        }

        public virtual async Task DeleteAsync(TId id)
        {
            await Collection.DeleteOneAsync(entity => entity.Id.Equals(id)).ConfigureAwait(false);
        }

        private async Task<TEntity?> FindUnsafeAsync(TId id)
        {
            return await (await Collection.FindAsync(entity => entity.Id.Equals(id))).SingleOrDefaultAsync().ConfigureAwait(false);
        }
    }
}
