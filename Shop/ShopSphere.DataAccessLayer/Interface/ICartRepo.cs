using ShopSphere.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSphere.DataAccessLayer.Interface
{
    public interface ICartRepo
    {
        Task<Cart> AddtoCartRepo(Cart neone);

        Task<Cart> checkExitsCartRepo(int userID);

        Task<Cart> UpdateCartRepo(Cart cart);

        Task CheckProductQuanity(IEnumerable<CartItem> ProuductsIDs);

        Task ReduceQuantity(IEnumerable<CartItem> items);

        Task<IEnumerable<CartItem>> GetAllCart(int id);
    }
}
