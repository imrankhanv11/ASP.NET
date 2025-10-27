using LibararyManagement.Data.Data;
using LibararyManagement.Data.Interface;
using LibararyManagement.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibararyManagement.Data.Repository
{
    public class AccountRepo : IAccountRepo
    {
        private readonly LibararyContext _dbContext;

        public AccountRepo(LibararyContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> AddNewUserRepo(User user)
        {
            _dbContext.Users.Add(user);

            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<User> ExtistingUserCheckRepo(string Email)
        {
            var result = await _dbContext.Users.Include(s=> s.Role).Where(s=> s.Email == Email).FirstOrDefaultAsync();

            return result;
        }
    }
}
