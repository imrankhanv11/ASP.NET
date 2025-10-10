using ECommerce.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccessLayer.Inteface
{
    public interface IAuthRepo
    {
        Task<bool> RegisterUserRepo(User dto);

        Task<User> CheckUserExitsRepo(string Email);
    }
}
