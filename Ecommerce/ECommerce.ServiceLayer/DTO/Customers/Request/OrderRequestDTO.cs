using ECommerce.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ServiceLayer.DTO.Customers.Request
{
    public class OrderRequestDTO
    {
        public int UserId { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = null!;

        public IEnumerable<OrderItemsRequestDTO> OrderItems { get; set; }
    }
}
