using System.Linq.Expressions;
using Company.Framework.Core.Monad;
using Company.Framework.Data.Entity;

namespace Company.Framework.Data.Repository
{
    public interface IRepository<TEntity, in TId> : IRepository where TEntity : CoreEntity<TId> where TId : struct
    {
        IAsyncEnumerable<TEntity> FindAllAsync(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null);
        Task<Optional<TEntity>> FindAsync(TId id);
        Task InsertAsync(TEntity entity);
        Task InsertManyAsync(IEnumerable<TEntity> entities);
        Task UpdateAsync(TEntity entity);
        Task UpdateManyAsync(IEnumerable<TEntity> entities);
        Task DeleteManyAsync(Expression<Func<TEntity, bool>> filter);
        Task DeleteAsync(TId id);
    }

    public interface IRepository { }
}
