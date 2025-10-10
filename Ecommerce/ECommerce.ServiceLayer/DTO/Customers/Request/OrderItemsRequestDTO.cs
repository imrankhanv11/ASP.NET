using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ServiceLayer.DTO.Customers.Request
{
    public class OrderItemsRequestDTO
    {
        public int OrderId { get; set; }

        [Required(ErrorMessage = "can't be null")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "can't be null")]
        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
