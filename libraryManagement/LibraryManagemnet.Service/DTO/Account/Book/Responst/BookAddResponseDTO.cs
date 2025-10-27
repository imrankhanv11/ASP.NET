using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Service.DTO.Account.Book.Responst
{
    public class BookAddResponseDTO
    {
        public int Id { get; set; }

        public string BookName { get; set; } = null!;

        public string Author { get; set; } = null!;

        public int Price { get; set; }

        public int Stock { get; set; }

        public int Category { get; set; }

        public int CategoryId { get; set; }

        public string? PictureLink { get; set; }
    }
}
