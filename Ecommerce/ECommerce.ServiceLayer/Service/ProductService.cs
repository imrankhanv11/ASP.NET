using AutoMapper;
using ECommerce.DataAccessLayer.Inteface;
using ECommerce.DataAccessLayer.Models;
using ECommerce.ServiceLayer.DTO.Products.Request;
using ECommerce.ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ServiceLayer.Service
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepo<Product> _productRepo;
        private readonly IMapper _mapper;

        public ProductService(IGenericRepo<Product> productRepo, IMapper mapper)
        {
            _productRepo = productRepo;
            _mapper = mapper;
        }


        public async Task<int> AddNewProductService(AddNewProductDTO dto)
        {
            var requestProduct = _mapper.Map<Product>(dto);

            var output = await _productRepo.AddAsync(requestProduct);

            return output.ProductId;
        }
    }
}
