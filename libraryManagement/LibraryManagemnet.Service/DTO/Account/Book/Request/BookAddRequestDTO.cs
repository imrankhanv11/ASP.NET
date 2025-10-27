using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LibraryManagement.Service.DTO.Account.Book.Request
{
    public class BookAddRequestDTO
    {
        public string BookName { get; set; } = null!;

        public string Author { get; set; } = null!;

        public int Price { get; set; }

        public int Stock { get; set; }

        public int CategoryId { get; set; }

        public IFormFile? PictureFile { get; set; }
    }
}
