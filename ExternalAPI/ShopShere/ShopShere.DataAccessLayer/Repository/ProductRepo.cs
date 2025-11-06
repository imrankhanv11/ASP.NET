using Microsoft.EntityFrameworkCore;
using ShopSphere.DataAccessLayer.Data;
using ShopSphere.DataAccessLayer.Interface;
using ShopSphere.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSphere.DataAccessLayer.Repository
{
    public class ProductRepo : IProductRepo
    {
        private readonly ShopSphereContext _dbContext;

        public ProductRepo(ShopSphereContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Product> AddNewProductRepo(Product product)
        {
            await _dbContext.Products.AddAsync(product);

            await _dbContext.SaveChangesAsync();

            return product;
        }

        public async Task<bool> CheckProductExitsRepo(string name)
        {
            var value = await _dbContext.Products.FirstOrDefaultAsync(s=> s.Name == name);

            if (value == null)
            {
                return true;
            }

            return false;
        }

        public async Task<Product> checkProductRepo(int id)
        {
            var result = await _dbContext.Products.FindAsync(id);

            return result;
        }

        public async Task<IEnumerable<Product>> GetAllRepo()
        {
            var allProducts = await _dbContext.Products.ToListAsync();

            return allProducts;
        }

        public async Task UpdateQuanity(Product product)
        {
            _dbContext.Products.Update(product);

            await _dbContext.SaveChangesAsync();

        }

        public async Task<Product> GetProductById(int productId)
        {
            var product = await _dbContext.Products
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {productId} not found");
            }

            return product;
        }

        public async Task<Product> UpdateProduct(Product product)
        {
             _dbContext.Update(product);
            await _dbContext.SaveChangesAsync();

            return product;
        }

    }
}
