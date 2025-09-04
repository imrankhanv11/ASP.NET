using Web_API_Practice.DTO;
using Web_API_Practice.Models;

namespace Web_API_Practice.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GellAllProducts();

        Task Addproduct(Product Product);

        Task<bool> DeleteProduct(int id);
    }
}
