using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSphere.ServiceLayer.DTO.Cart.Request
{
    public  class CartDeleteRequest
    {
        public int CartItemID { get; set; }
        public int Quantity { get; set; }

        public int ProductId { get; set; }
    }
}
