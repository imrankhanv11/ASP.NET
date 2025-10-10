using ECommerce.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccessLayer.Inteface
{
    public interface ICustomerRepo
    {
        Task<bool> OrderProductsRepo(Order newone);

        Task<bool> AddtoCartRepo(Cart neone);

        Task<Cart> checkExitsCartRepo(int userID);

        Task<bool> UpdateCartRepo(Cart cart);
    }
}
