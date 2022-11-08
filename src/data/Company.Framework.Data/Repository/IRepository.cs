using System.Linq.Expressions;
using Company.Framework.Data.Entity;
using LanguageExt;
using LanguageExt.Common;

namespace Company.Framework.Data.Repository
{
    public interface IRepository<TEntity, in TId> : IRepository where TEntity : CoreEntity<TId>
    {
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null);
        Task<TEntity?> GetByIdAsync(TId id);
        Task InsertAsync(TEntity entity);
        Task InsertManyAsync(IEnumerable<TEntity> entities);
        Task UpdateAsync(TEntity entity);
        Task DeleteManyAsync(Expression<Func<TEntity, bool>> filter);
        Task DeleteAsync(TId id);
    }

    public interface IOptionalRepository<TEntity, in TId> : IRepository where TEntity : CoreEntity<TId>
    {
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null);
        Task<Option<TEntity>> GetByIdAsync(TId id);
        Task InsertAsync(TEntity entity);
        Task InsertManyAsync(IEnumerable<TEntity> entities);
        Task UpdateAsync(TEntity entity);
        Task DeleteManyAsync(Expression<Func<TEntity, bool>> filter);
        Task DeleteAsync(TId id);
    }

    public interface IRepository { }
}
