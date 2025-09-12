using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookManagement.DataAccessLayer.Data;
using BookManagement.DataAccessLayer.Interfaces;
using BookManagement.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.DataAccessLayer.Repositories
{
    public class LoginRepo : ILoginRepo
    {
        private readonly BookContext _dbContext;

        public LoginRepo(BookContext _context)
        {
            _dbContext = _context;
        }

        public async Task<User> CheckUserExitsRepo(string userName)
        {
            var userexit = await _dbContext.Users
                .Include(s=> s.Role)
                .FirstOrDefaultAsync(s=> s.Username == userName);

            return userexit;
        }

        public async Task<bool> RegisterUserRepo(User user)
        {
            _dbContext.Users.Add(user);

            var value = await _dbContext.SaveChangesAsync();

            if(value > 0) return true;

            return false;
        }
    }
}
