using ShopSphere.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSphere.ServiceLayer.DTO.Cart.Request
{
    public class AddtoCartDTO
    {
        public List<AddCartItemsDTO> CartItems { get; set; } 
    }
}
