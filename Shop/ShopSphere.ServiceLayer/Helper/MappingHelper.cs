using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ShopSphere.DataAccessLayer.Models;
using ShopSphere.ServiceLayer.DTO.Cart.Request;
using ShopSphere.ServiceLayer.DTO.Cart.Response;
using ShopSphere.ServiceLayer.DTO.Products.Request;
using ShopSphere.ServiceLayer.DTO.Products.Response;

namespace ShopSphere.ServiceLayer.Helper
{
    public class MappingHelper : Profile
    {
        public MappingHelper()
        {
            CreateMap<AddProductDTO, Product>().ReverseMap();

            CreateMap<AddCartItemsDTO, CartItem>().ReverseMap();

            CreateMap<AddtoCartDTO, Cart>().ReverseMap();

            // Mapper for Get Cart Items
            CreateMap<CartItem, GetAllCart>()
                .ForMember(des => des.Name, option => option.MapFrom(src => src.Product.Name))
                .ForMember(des => des.Price, option => option.MapFrom(src => src.Product.Price));


            CreateMap<Product, GetAllProductsDTO>();
        }
    }
}


