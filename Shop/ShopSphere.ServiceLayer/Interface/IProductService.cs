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
        Task<int> AddNewProductService(AddProductDTO dto);

        Task<IEnumerable<GetAllProductsDTO>> GetAllService();

        Task UpdateQuantity(int id, int quantity);
    }
}
