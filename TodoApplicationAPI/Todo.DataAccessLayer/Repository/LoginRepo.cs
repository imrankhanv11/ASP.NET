using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.DataAccessLayer.Data;
using Todo.DataAccessLayer.Interface;
using Todo.ModelLayer.DTO.Request;
using Todo.ModelLayer.Models;

namespace Todo.DataAccessLayer.Repository
{
    public class LoginRepo : ILoginRepo
    {
        private readonly TodoContext _dbContext;

        public LoginRepo(TodoContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        public async Task AddRefeshRepo(RefershToken dto)
        {
            _dbContext.RefreshToken.Add(dto);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<User> CheckPasswordUser(LoginDTO dto)
        {
            var value = await _dbContext.Users.Where(s=> s.Email == dto.Email && s.Password == dto.Password).FirstOrDefaultAsync();

            return value;
        }
        public async Task<RefershToken> CheckRefreshToken(string token)
        {
            var value = await _dbContext.RefreshToken
                .FirstOrDefaultAsync(t => t.Token == token && !t.IsRevoked);

            return value;
        }

        public async Task UpdateRevokeRefreshToken(RefershToken dto)
        {
            dto.IsRevoked = true;

            await _dbContext.SaveChangesAsync();
        }

    }
}
