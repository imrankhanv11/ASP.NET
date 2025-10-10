using ECommerce.DataAccessLayer.Data;
using ECommerce.DataAccessLayer.Inteface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccessLayer.Repository
{
    public class GenericRepo<T> : IGenericRepo<T> where T : class
    {
        private readonly ECommerceContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public GenericRepo(ECommerceContext context)
        {
            _dbContext = context;
            _dbSet = _dbContext.Set<T>();
        }
        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
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
