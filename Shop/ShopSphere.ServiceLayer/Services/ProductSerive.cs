using AutoMapper;
using Microsoft.Extensions.Logging;
using ShopSphere.DataAccessLayer.Interface;
using ShopSphere.DataAccessLayer.Models;
using ShopSphere.ServiceLayer.DTO.Products.Request;
using ShopSphere.ServiceLayer.DTO.Products.Response;
using ShopSphere.ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSphere.ServiceLayer.Services
{
    public class ProductSerive : IProductService
    {
        private readonly IProductRepo _repo;
        private readonly ILogger<ProductSerive> _logger;
        private readonly IMapper _mapper;

        public ProductSerive(IProductRepo repo, ILogger<ProductSerive> logger, IMapper mapper)
        {
            _repo = repo;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<int> AddNewProductService(AddProductDTO dto)
        {
            var checkProduct = await _repo.CheckProductExitsRepo(dto.Name);

            if (!checkProduct)
            {
                _logger.LogInformation("ProductName Fail");
                throw new ValidationException("Product Name Already Exits");
            }

            if(dto.Name.Trim().Length == 0)
            {
                _logger.LogInformation("ProductName length Fail");
                throw new ValidationException("Product can't be Empty");
            }

            if(dto.Description.Trim().Length == 0)
            {
                _logger.LogInformation("Description Fail");
                throw new ValidationException("Description can't be Empty");
            }

            if(dto.Price <= 0)
            {
                _logger.LogInformation("Prize Fail");
                throw new ValidationException("Prize Can't be Zero or Negative");
            }

            if(dto.Stock <= 0)
            {
                _logger.LogInformation("Stock Fail");
                throw new ValidationException("Stok Can't be Zero or Negative");
            }

            var Product = _mapper.Map<Product>(dto);
            _logger.LogInformation("Mapper Compeleted");

            var output = await _repo.AddNewProductRepo(Product);
            _logger.LogInformation("New product Added Succesfully");

            return output.ProductId;
        }

        public async Task<IEnumerable<GetAllProductsDTO>> GetAllService()
        {
            var output = await _repo.GetAllRepo();

            var result = _mapper.Map<IEnumerable<GetAllProductsDTO>>(output);

            return result;
        }

        public async Task UpdateQuantity(int id, int quantity)
        {
            var product = await _repo.checkProductRepo(id);

            if (product == null)
            {
                throw new KeyNotFoundException("Product not found");
            }

            product.Stock += quantity;

            await _repo.UpdateQuanity(product);
        }
    }
}
