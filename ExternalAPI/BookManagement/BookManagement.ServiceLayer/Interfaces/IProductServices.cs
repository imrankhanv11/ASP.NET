using BookManagement.DataAccessLayer.Models;
using BookManagement.ServiceLayer.DTO.Books.Request;
using BookManagement.ServiceLayer.DTO.Books.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.ServiceLayer.Interfaces
{
    public interface IProductServices
    {
        Task<IEnumerable<GetAllBooksDTO>> GetAllBooksService();

        Task<GetOneBookDTO> GetOneBookService(int id);

        Task<AddBookResponseDTO> AddBookService(AddBookDTO book);

        Task<bool> DeleteBookService(int id);

        Task<GetOneBookByNameDTO> GetOnebookService(string name);
    }
}
