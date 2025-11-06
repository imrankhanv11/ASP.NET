using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSphere.ServiceLayer.DTO.Products.Request
{
    public class UpdateProductDTO
    {

        [Required(ErrorMessage = "Name filed is Required")]
        [MinLength(3, ErrorMessage = "Minimum 3 Char Reqired")]
        public string Name { get; set; } = null!;


        [Required(ErrorMessage = "Description filed is Required")]
        [MinLength(5, ErrorMessage = "Minimum 5 Char Reqired")]
        public string? Description { get; set; }


        [Required(ErrorMessage = "Prize filed is Required")]
        public decimal Price { get; set; }


        [Required(ErrorMessage = "Stock filed is Required")]
        public int Stock { get; set; }
    }
}
