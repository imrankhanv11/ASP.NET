using ShopSphere.DataAccessLayer.Models;
using ShopSphere.ServiceLayer.DTO.Products.Request;
using ShopSphere.ServiceLayer.DTO.Products.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSphere.ServiceLayer.Interface
{
    public interface IProductService
    {
        Task<GetAllProductsDTO> AddNewProductService(AddProductDTO dto);

        Task<IEnumerable<GetAllProductsDTO>> GetAllService();

        Task UpdateQuantity(int id, int quantity);

        Task<UpdateProductRespontDTO> UpdateProductService(UpdateProductDTO dto, int id);
    }
}
