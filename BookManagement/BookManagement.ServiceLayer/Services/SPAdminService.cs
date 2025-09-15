using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookManagement.DataAccessLayer.Interfaces;
using BookManagement.DataAccessLayer.Models;
using BookManagement.ServiceLayer.Interfaces;

namespace BookManagement.ServiceLayer.Services
{
    public class SPAdminService : ISPAdminService
    {
        private readonly IGenericRepository<User> _userRepo;

        public SPAdminService(IGenericRepository<User> userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<bool> AddAdminService(int id)
        {
            var value = await _userRepo.GetByIdAsync(id);

            if(value == null)
            {
                throw new KeyNotFoundException("Id not found");
            }

            value.RoleId = 3;

            await _userRepo.UpdateAsync(value);

            return true;
        }
    }
}
