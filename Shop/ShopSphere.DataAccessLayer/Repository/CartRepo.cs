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

        public async Task CheckProductQuanity(IEnumerable<CartItem> ProuductsIDs)
        {
            foreach(var item in ProuductsIDs)   
            {
                var checkID = await _dbContext.Products.FirstOrDefaultAsync(s => s.ProductId == item.ProductId);

                if (checkID == null)
                {
                    throw new ValidationException($"Product ID: {checkID.ProductId} not found");
                }

                if(checkID.Stock < item.Quantity)
                {
                    throw new ValidationException($"Product ID: {checkID.ProductId} Out of stock");
                }
            }
        }

        public async Task ReduceQuantity(IEnumerable<CartItem> items)
        {
            foreach(var item in items)
            {
                var products = await _dbContext.Products.FirstOrDefaultAsync(s=> s.ProductId == item.ProductId);

                products.Stock = products.Stock - item.Quantity;

            }


            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<CartItem>> GetAllCart(int id)
        {
            var output = await _dbContext.CartItems
                .Include(s => s.Product)
                .ThenInclude(s => s.CartItems)
                .Where(s => s.Cart.UserId == id)
                .ToListAsync();

            return output;
        }
    }
}
