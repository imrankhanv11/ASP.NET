using BookManagement.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookManagement.ServiceLayer.DTO.Books.Response;

namespace BookManagement.ServiceLayer.DTO.Category.Response
{
    public class CatWithProductsDTO
    {
        public string CategoryName { get; set; } = null!;

        public List<CatWithProductsProductsDTO> Books { get; set; } 
    }
}
