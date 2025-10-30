using BookManagement.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.DataAccessLayer.Interfaces
{
    public interface ILoginRepo
    {
        // Register
        Task<User> CheckUserExitsRepo(string userName);
        Task<bool> RegisterUserRepo(User user);

    }
}
