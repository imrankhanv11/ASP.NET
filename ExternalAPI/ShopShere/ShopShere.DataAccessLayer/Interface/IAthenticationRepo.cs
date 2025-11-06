using ShopSphere.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSphere.DataAccessLayer.Interface
{
    public interface IAthenticationRepo
    {
        Task<bool> RegisterUserRepo(User dto);

        Task<User> CheckUserExitsRepo(string Email);
    }
}
