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

        Task<bool> CheckProductQuanity(int ProuductsIDs);

        Task ReduceQuantity(int items);

        Task<IEnumerable<CartItem>> GetAllCart(int id);

        Task<CartItem> GetCartItemById(int cartItemId);

        Task UpdateCartItem(CartItem item);

        Task DeleteCartItem(int cartItemId);

        Task IncreaseProductQuantity(int items);
    }
}
