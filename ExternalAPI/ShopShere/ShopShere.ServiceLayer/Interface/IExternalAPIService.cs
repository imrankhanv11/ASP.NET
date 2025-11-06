using ShopSphere.ServiceLayer.DTO.ExternalAPI.Request;
using ShopSphere.ServiceLayer.DTO.ExternalAPI.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSphere.ServiceLayer.Interface
{
    public interface IExternalAPIService
    {
        Task<List<GetAllBookResponseDTO>> GetAllBooks();

        Task<GetAllBookResponseDTO?> GetBookById(int id);

        Task<GetOneBookByNameResponseDTO?> GetOneBookByName(string name);

        Task<bool> DeleteBookId(int id);

        Task<int> AddBook(AddBookRequestDTO dto);
    }
}
