using System.Linq.Expressions;
using Company.Framework.Data.Entity;
using Company.Framework.Data.Mongo.Context;
using Company.Framework.Data.Repository;
using LanguageExt;
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

        public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? predicate = default, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = default)
        {
            return (await Collection.FindAsync(predicate)).ToEnumerable();
        }

        public virtual async Task<TEntity?> GetByIdAsync(TId id)
        {
            return await (await Collection.FindAsync(entity => entity.Id.Equals(id))).SingleOrDefaultAsync();
        }

        public virtual async Task InsertAsync(TEntity entity)
        {
            await Collection.InsertOneAsync(entity);
        }
        public virtual async Task InsertManyAsync(IEnumerable<TEntity> entities)
        {
            await Collection.InsertManyAsync(entities);
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            await Collection.ReplaceOneAsync(e => e.Id.Equals(entity.Id), entity);
        }

        public virtual async Task DeleteManyAsync(Expression<Func<TEntity, bool>> filter)
        {
            await Collection.DeleteManyAsync(filter);
        }


        public virtual async Task DeleteAsync(TId id)
        {
            await Collection.DeleteOneAsync(entity => entity.Id.Equals(id));
        }
    }

    public abstract class CoreOptionalMongoRepository<TEntity, TId> : IOptionalRepository<TEntity, TId> where TEntity : CoreEntity<TId> where TId : struct
    {
        protected readonly IMongoCollection<TEntity> Collection;

        protected CoreOptionalMongoRepository(IMongoDbContext dbContext)
        {
            Collection = dbContext.GetCollection<TEntity>();
        }
        protected CoreOptionalMongoRepository(IMongoDbContext dbContext, string collectionName)
        {
            Collection = dbContext.GetCollection<TEntity>(collectionName);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? predicate = default, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = default)
        {
            return (await Collection.FindAsync(predicate)).ToEnumerable();
        }

        public virtual async Task<Option<TEntity>> GetByIdAsync(TId id)
        {
            return await (await Collection.FindAsync(entity => entity.Id.Equals(id))).SingleOrDefaultAsync();
        }
        public virtual async Task InsertAsync(TEntity entity)
        {
            await Collection.InsertOneAsync(entity);
        }
        public virtual async Task InsertManyAsync(IEnumerable<TEntity> entities)
        {
            await Collection.InsertManyAsync(entities);
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            await Collection.ReplaceOneAsync(e => e.Id.Equals(entity.Id), entity);
        }

        public virtual async Task DeleteManyAsync(Expression<Func<TEntity, bool>> filter)
        {
            await Collection.DeleteManyAsync(filter);
        }


        public virtual async Task DeleteAsync(TId id)
        {
            await Collection.DeleteOneAsync(entity => entity.Id.Equals(id));
        }
    }
}
