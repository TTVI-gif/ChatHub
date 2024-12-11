using ChatHub.Domain.Entities;
using System.Linq.Expressions;

namespace ChatHub.Application.IRepository
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        Task<IList<TEntity>> GetAllAsync();

        Task<TEntity?> GetByIdAsync(Guid id);

        Task AddAsync(TEntity entity);

        void Update(TEntity entity);

        void UpdateRange(ICollection<TEntity> entities);

        void SoftRemove(TEntity entity);

        Task AddRangeAsync(ICollection<TEntity> entities);

        void SoftRemoveRange(ICollection<TEntity> entities);
        Task<bool> ExistAnyAsync(Expression<Func<TEntity, bool>> expression);

         Task<TEntity?> FindAsync(Guid id, params Expression<Func<TEntity, object?>>[] includes);

        Task<TEntity?> GetByIdAsync(Guid id, params Expression<Func<TEntity, object?>>[] includes);
    }
}
