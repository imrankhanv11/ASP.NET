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
    public class AuthenticationRepo : IAthenticationRepo
    {
        private readonly ShopSphereContext _dbContext;

        public AuthenticationRepo(ShopSphereContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> CheckUserExitsRepo(string Email)
        {
            var FindUser = await _dbContext.Users.Include(r => r.Role).FirstOrDefaultAsync(s => s.Email == Email);

            return FindUser;
        }

        public async Task<bool> RegisterUserRepo(User dto)
        {
            _dbContext.Users.Add(dto);

            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
