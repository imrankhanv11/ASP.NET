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
    public class AuthRepo : IAuthRepo
    {
        private readonly ECommerceContext _dbContext;

        public AuthRepo(ECommerceContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> CheckUserExitsRepo(string Email)
        {
            var value = await _dbContext.Users.Include(r=> r.Role).FirstOrDefaultAsync(s=> s.Email == Email);

            return value;
        }

        public async Task<bool> RegisterUserRepo(User dto)
        {
            _dbContext.Users.Add(dto);  

            await _dbContext.SaveChangesAsync();

            return true;
        }


    }
}
