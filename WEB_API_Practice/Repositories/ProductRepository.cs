using Microsoft.EntityFrameworkCore;
using Web_API_Practice.Data;
using Web_API_Practice.DTO;
using Web_API_Practice.Interfaces;
using Web_API_Practice.Models;

namespace Web_API_Practice.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly NorthWindContext _dbContext;

        public ProductRepository(NorthWindContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Product>> GellAllProducts()
        {
            return await _dbContext.Products.ToListAsync();
        }


        public async Task Addproduct(Product Product)
        {
            _dbContext.Products.Add(Product);

            await _dbContext.SaveChangesAsync();
        }
        public async Task<bool> DeleteProduct(int id)
        {
            try
            {
                var result = await _dbContext.Products.FindAsync(id);

                if (result == null) return false;

                _dbContext.Products.Remove(result);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}