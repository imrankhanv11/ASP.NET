using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ServiceLayer.DTO.Products.Request
{
    public class AddNewProductDTO
    {
        [Required(ErrorMessage = "Name filed required")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Description filed required")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Price filed required")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock filed required")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "CategoryID filed required")]
        public int CategoryId { get; set; }
    }
}
