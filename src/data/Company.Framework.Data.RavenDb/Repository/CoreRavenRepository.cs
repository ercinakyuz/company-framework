using System.Linq.Expressions;
using Company.Framework.Data.Entity;
using Company.Framework.Data.Raven.Context;
using Company.Framework.Data.Repository;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;

namespace Company.Framework.Data.Raven.Repository;

public abstract class CoreRavenRepository<TEntity, TId> : IRepository<TEntity, TId> where TEntity : CoreEntity<TId>
{
    protected readonly IRavenDbContext DbContext;

    protected CoreRavenRepository(IRavenDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public virtual async IAsyncEnumerable<TEntity> FindAllAsync(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
    {
        using var session = DbContext.OpenSessionAsync();
        
        var query = session.Query<TEntity>();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        var results = await query.ToListAsync().ConfigureAwait(false);

        foreach (var entity in results)
        {
            yield return entity;
        }
    }

    public virtual async Task<Core.Monad.Optional<TEntity>> FindAsync(TId id)
    {
        using var session = DbContext.OpenSessionAsync();
        var entity = await session.LoadAsync<TEntity>(FormatId(id)).ConfigureAwait(false);
        return Core.Monad.Optional<TEntity>.OfNullable(entity);
    }

    public virtual async Task InsertAsync(TEntity entity)
    {
        using var session = DbContext.OpenSessionAsync();
        await session.StoreAsync(entity).ConfigureAwait(false);
        await session.SaveChangesAsync().ConfigureAwait(false);
    }

    public virtual async Task InsertManyAsync(IEnumerable<TEntity> entities)
    {
        using var session = DbContext.OpenSessionAsync();
        foreach (var entity in entities)
        {
            await session.StoreAsync(entity).ConfigureAwait(false);
        }
        await session.SaveChangesAsync().ConfigureAwait(false);
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        using var session = DbContext.OpenSessionAsync();
        await session.StoreAsync(entity).ConfigureAwait(false);
        await session.SaveChangesAsync().ConfigureAwait(false);
    }

    public virtual async Task UpdateManyAsync(IEnumerable<TEntity> entities)
    {
        using var session = DbContext.OpenSessionAsync();
        foreach (var entity in entities)
        {
            await session.StoreAsync(entity).ConfigureAwait(false);
        }
        await session.SaveChangesAsync().ConfigureAwait(false);
    }

    public virtual async Task DeleteManyAsync(Expression<Func<TEntity, bool>> filter)
    {
        using var session = DbContext.OpenSessionAsync();
        var entities = await session.Query<TEntity>()
            .Where(filter)
            .ToListAsync()
            .ConfigureAwait(false);

        foreach (var entity in entities)
        {
            session.Delete(entity);
        }
        await session.SaveChangesAsync().ConfigureAwait(false);
    }

    public virtual async Task DeleteAsync(TId id)
    {
        using var session = DbContext.OpenSessionAsync();
        session.Delete(FormatId(id));
        await session.SaveChangesAsync().ConfigureAwait(false);
    }

    protected virtual string FormatId(TId id)
    {
        return $"{typeof(TEntity).Name}/{id}";
    }
}
