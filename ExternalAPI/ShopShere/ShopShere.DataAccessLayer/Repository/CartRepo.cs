using Microsoft.EntityFrameworkCore;
using ShopSphere.DataAccessLayer.Data;
using ShopSphere.DataAccessLayer.Interface;
using ShopSphere.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSphere.DataAccessLayer.Repository
{
    public class CartRepo : ICartRepo
    {
        private readonly ShopSphereContext _dbContext;

        public CartRepo(ShopSphereContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Cart> AddtoCartRepo(Cart neone)
        {
            await _dbContext.Carts.AddAsync(neone);

            await _dbContext.SaveChangesAsync();

            return neone;
        }

        public async Task<Cart> UpdateCartRepo(Cart cart)
        {
            _dbContext.Carts.Update(cart);
            await _dbContext.SaveChangesAsync();
            return cart;
        }

        public async Task<Cart> checkExitsCartRepo(int userID)
        {
            var output = await _dbContext.Carts.Include(s => s.CartItems).FirstOrDefaultAsync(s => s.UserId == userID);

            return output;
        }

        public async Task<bool> CheckProductQuanity(int ProuductsIDs)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductId == ProuductsIDs);
            if (product == null)
                throw new KeyNotFoundException("Product not found");

            return product.Stock > 0;

        }

        public async Task<IEnumerable<CartItem>> GetAllCart(int id)
        {
            var output = await _dbContext.CartItems
                .Include(s => s.Product)
                .Where(s => s.Cart.UserId == id)
                .ToListAsync();

            return output;
        }

        public async Task<CartItem> GetCartItemById(int cartItemId)
        {
            return await _dbContext.CartItems
                .Include(ci => ci.Cart)
                .Include(ci => ci.Product)
                .FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId);
        }

        public async Task UpdateCartItem(CartItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _dbContext.CartItems.Update(item);  
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteCartItem(int cartItemId)
        {
            var item = await _dbContext.CartItems.FindAsync(cartItemId);
            if (item == null)
                throw new KeyNotFoundException("Cart item not found");

            _dbContext.CartItems.Remove(item);
            await _dbContext.SaveChangesAsync();
        }

        public async Task IncreaseProductQuantity(int productId)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
            if (product == null)
                throw new KeyNotFoundException("Product not found");

            product.Stock += 1;
            await _dbContext.SaveChangesAsync();
        }

        public async Task ReduceQuantity(int productId)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
            if (product == null)
                throw new KeyNotFoundException("Product not found");

            if (product.Stock < 1)
                throw new ValidationException("Not enough stock");

            product.Stock -= 1;
            await _dbContext.SaveChangesAsync();
        }

    }
}
