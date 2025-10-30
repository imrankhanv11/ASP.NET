using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.ServiceLayer.DTO.Books.Response
{
    public class GetOneBookByNameDTO
    {

        public string Title { get; set; } = null!;

        public string Author { get; set; } = null!;

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public string CategoryName { get; set; }
    }
}
