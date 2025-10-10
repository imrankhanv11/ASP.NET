using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.DataAccessLayer.Models;
using ECommerce.ServiceLayer.DTO.Customers.Request;
using ECommerce.ServiceLayer.DTO.Products.Request;

namespace ECommerce.ServiceLayer.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<OrderItemsRequestDTO, OrderItem>().ReverseMap();

            CreateMap<OrderRequestDTO, Order>().ReverseMap();

            CreateMap<CartItemsDTO, CartItem>().ReverseMap();

            CreateMap<CartAddDTO, Cart>().ReverseMap();

            CreateMap<AddNewProductDTO, Product>().ReverseMap();
        }
    }
}
