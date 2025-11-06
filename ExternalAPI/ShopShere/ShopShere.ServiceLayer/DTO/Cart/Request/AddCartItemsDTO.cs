using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSphere.ServiceLayer.DTO.Cart.Request
{
    public class AddCartItemsDTO
    {
        [Required(ErrorMessage ="Product ID is Required")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product Quantity is Requried")]
        public int Quantity { get; set; }
    }
}
