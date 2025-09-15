using BookManagement.DataAccessLayer.Interfaces;
using BookManagement.DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;
using BookManagement.DataAccessLayer.Models;

namespace BookManagement.DataAccessLayer.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly BookContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(BookContext context)
        {
            _dbContext = context;
            _dbSet = _dbContext.Set<T>();
        }
        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddRangeAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var value = await _dbSet.FindAsync(id);

            if (value != null)
            {
                _dbSet.Remove(value);   
                await _dbContext.SaveChangesAsync();
            }

        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);

            await _dbContext.SaveChangesAsync();

            return entity;
        }

    }
}
