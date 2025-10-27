using LibararyManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibararyManagement.Data.Interface
{
    public interface IAccountRepo
    {
        Task<User> ExtistingUserCheckRepo(string Email);

        Task<User> AddNewUserRepo(User user);
    }
}
