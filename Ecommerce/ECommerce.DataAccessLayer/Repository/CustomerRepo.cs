using ECommerce.DataAccessLayer.Data;
using ECommerce.DataAccessLayer.Inteface;
using ECommerce.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccessLayer.Repository
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly ECommerceContext _dbContext;

        public CustomerRepo(ECommerceContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddtoCartRepo(Cart neone)
        {
            await _dbContext.Carts.AddAsync(neone);

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateCartRepo(Cart cart)
        {
            _dbContext.Carts.Update(cart);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Cart> checkExitsCartRepo(int userID)
        {
            var output = await _dbContext.Carts.Include(s=> s.CartItems).FirstOrDefaultAsync(s=> s.UserId == userID);

            return output;
        }

        public async Task<bool> OrderProductsRepo(Order newone)
        {
            await _dbContext.Orders.AddAsync(newone);

            await _dbContext.SaveChangesAsync();

            return true;

        }
    }
}
