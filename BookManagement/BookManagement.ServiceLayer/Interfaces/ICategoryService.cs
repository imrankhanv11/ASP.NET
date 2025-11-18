using BookManagement.ServiceLayer.DTO.Category.Request;
using BookManagement.ServiceLayer.DTO.Category.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.ServiceLayer.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<GetAllCategoryDTO>> GetAllCatService();

        Task<GetOneCatDTO> GetOneCatService(int id);

        Task<int> AddNewCatService(AddCatDto dto);

        Task<bool> DeleteCatService(int id);

        Task<IEnumerable<CatWithProductsDTO>> catWithProductService(int id);

        Task CatUpdateService(UpdateCatRequestDTO dto);
    }
}
