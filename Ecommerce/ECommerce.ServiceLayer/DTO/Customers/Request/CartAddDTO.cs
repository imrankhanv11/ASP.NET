using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ServiceLayer.DTO.Customers.Request
{
    public class CartAddDTO
    {

        public int UserId { get; set; }

        public List<CartItemsDTO> CartItems { get; set; }
    }
}
