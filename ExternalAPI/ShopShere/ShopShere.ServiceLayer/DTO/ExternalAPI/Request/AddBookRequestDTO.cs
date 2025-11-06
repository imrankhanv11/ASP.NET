using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSphere.ServiceLayer.DTO.ExternalAPI.Request
{
    public class AddBookRequestDTO
    {
        public string Title { get; set; } = null!;

        public string Author { get; set; } = null!;

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public int CategoryId { get; set; }
    }
}
