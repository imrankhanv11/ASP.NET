using ShopSphere.DataAccessLayer.Models;
using ShopSphere.ServiceLayer.DTO.Cart.Request;
using ShopSphere.ServiceLayer.DTO.Cart.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSphere.ServiceLayer.Interface
{
    public interface ICartService
    {
        Task<CartItemDTO> AddtoCartService(AddCartItemsDTO dto, int UserId);

        Task<IEnumerable<GetAllCart>> GetAllCartService(int UserId);

        Task<decimal> GetTotalAmount(IEnumerable<GetAllCart> cart);

        Task<CartItem> UpdateCartItemQuantity(int cartItemId, UpdateCartItemDTO dto);

        Task<bool> cartDeleteDTO(CartDeleteRequest dto);

        Task<CartItemDTO> IncreaseQuantity(int cartItemId);
        Task<CartItemDTO> DecreaseQuantity(int cartItemId);
    }
}
