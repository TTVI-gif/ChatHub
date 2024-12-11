using ChatHub.Application.IRepository;
using ChatHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace ChatHub.Infrastructures.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        protected DbSet<TEntity> _dbSet;
        public GenericRepository(ChatDbContext context)
        {
            _dbSet = context.Set<TEntity>();
        }
        public async Task AddAsync(TEntity entity)
        {
            entity.CreationDate = DateTime.Now;
            entity.ModificationDate = DateTime.Now;
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(ICollection<TEntity> entities)
        {
            foreach (var item in entities)
            {
                item.CreationDate = DateTime.Now;
                item.ModificationDate = DateTime.Now;
            }
            await _dbSet.AddRangeAsync(entities);
        }

        public async Task<bool> ExistAnyAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _dbSet.AnyAsync(expression);
        }

        public async Task<TEntity?> FindAsync(Guid id, params Expression<Func<TEntity, object?>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;
            query = query.Where(x => x.Id == id);
            if (includes != null)
            {
                foreach (Expression<Func<TEntity, object?>> include in includes)
                    query = query.Include(include);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<IList<TEntity>> GetAllAsync()
        {
            return await _dbSet.Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<TEntity?> GetByIdAsync(Guid id, params Expression<Func<TEntity, object?>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;

            if (includes != null)
            {
                foreach (Expression<Func<TEntity, object?>> include in includes)
                    query = query.Include(include);
            }
            return await query.FirstOrDefaultAsync();
        }

        public void SoftRemove(TEntity entity)
        {
            entity.IsDeleted = true;
            entity.ModificationDate = DateTime.Now;
            _dbSet.Update(entity);
        }

        public void SoftRemoveRange(ICollection<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
            }
            _dbSet.UpdateRange(entities);
        }

        public void Update(TEntity entity)
        {
            entity.ModificationDate = DateTime.Now;
            _dbSet.Update(entity);
        }

        public void UpdateRange(ICollection<TEntity> entities)
        {
            foreach (var item in entities)
            {
                item.ModificationDate = DateTime.Now;
            }
            _dbSet.UpdateRange(entities);
        }
    }
}
