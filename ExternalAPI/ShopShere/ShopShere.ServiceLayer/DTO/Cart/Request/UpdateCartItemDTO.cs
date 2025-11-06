using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSphere.ServiceLayer.DTO.Cart.Request
{
    public class UpdateCartItemDTO
    {
        public int? Increment { get; set; }  // e.g., +1
        public int? Decrement { get; set; }  // e.g., -1
    }

}
