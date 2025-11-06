using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSphere.ServiceLayer.DTO.ExternalAPI.Response
{
    public class GetAllBookResponseDTO
    {
        public int BookId { get; set; }

        public string Title { get; set; } = null!;

        public string Author { get; set; } = null!;

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public int CategoryId { get; set; }
    }
}
