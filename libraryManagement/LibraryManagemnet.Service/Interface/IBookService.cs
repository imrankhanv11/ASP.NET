using LibararyManagement.Data.Models;
using LibraryManagement.Service.DTO.Account.Book.Request;
using LibraryManagement.Service.DTO.Account.Book.Responst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Service.Interface
{
    public interface IBookService
    {
        Task<BookAddResponseDTO> AddBookService(BookAddRequestDTO dto, string pictureLink);

        Task<IEnumerable<BookGetAllResponseDTO>> GetAllService();

        Task<IEnumerable<CatGetAllResponseDTO>> GetAllServiceCat();

        Task DeleteBookService(int id);

        Task<string> GetOneBookByID(int id);

        Task<BookAddResponseDTO> UpdateBookService(int id, BookAddRequestDTO dto, string newImageLink);
    }
}
