using Web_API_Practice.DTO;
using Web_API_Practice.Interfaces;
using Web_API_Practice.Models;

namespace Web_API_Practice.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repositories;

        public ProductService(IProductRepository repositories)
        {
            _repositories = repositories;
        }

        public async Task<IEnumerable<AllProductsDTO>> GetAllProducts()
        {
            var Products = await _repositories.GellAllProducts();

            return Products.Select(s => new AllProductsDTO
            {
                ProductId = s.ProductId,
                ProductName = s.ProductName,
                UnitPrize = (decimal)s.UnitPrice
            });
        }


        public async Task AddProduct(AddProduct product)
        {
            var newProduct = new Product
            {
                ProductName = product.ProductName,
                UnitPrice = product.UnitPrize,
                CategoryId = product.CategoryID,
                SupplierId = product.ShipperID
            };

            await _repositories.Addproduct(newProduct);
        }

        public async Task<bool> DeleteProductService(int id)
        {
            return await _repositories.DeleteProduct(id);
        }
    }
}
