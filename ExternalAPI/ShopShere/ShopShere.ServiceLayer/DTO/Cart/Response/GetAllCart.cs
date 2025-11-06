using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSphere.ServiceLayer.DTO.Cart.Response
{
    public class GetAllCart
    {
        public int CartItemId { get; set; }      // optional, needed for increment/decrement
        public int ProductId { get; set; }       // needed to identify product
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }
    }

}
