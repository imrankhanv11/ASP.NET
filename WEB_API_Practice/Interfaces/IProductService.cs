using Web_API_Practice.DTO;

namespace Web_API_Practice.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<AllProductsDTO>> GetAllProducts();

        Task AddProduct(AddProduct product);

        Task<bool> DeleteProductService(int id);
    }
}
