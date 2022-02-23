using System.Linq.Expressions;
using JobPortalApi.Database;
using JobPortalApi.Database.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;

namespace JobPortalApi.Repositories
{
    public interface IGenericRepository<T> where T : BaseModel
    {
        Task<IEnumerable<T>> AllAsync();
        Task<T> GetByIdAsync(Guid id);
        Task<T> AddAsync(T entity);
        Task<bool> RemoveAsync(Guid id);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    }

    public class GenericRepository<T> : IGenericRepository<T> where T : BaseModel
    {
        private readonly DbSet<T> _dbSet;

        protected GenericRepository(DatabaseContext context)
        {
            _dbSet = context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> AllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<bool> RemoveAsync(Guid id)
        {
            var found = await GetByIdAsync(id);
            found.DeletedAt = DateTimeOffset.Now;
            return true;
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            var result = await _dbSet.FindAsync(id);
            if(result == null)
            {
                throw new Exception($"Entity {_dbSet.EntityType.Name} with ID: {id} was not found");
            }
            return result;
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);            
            return entity;
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            var result = await _dbSet.Where(predicate).ToListAsync();
            if (result == null)
            {
                throw new Exception($"Entity {_dbSet.EntityType.Name} with was not found");
            }
            return result;
        }
    }
}