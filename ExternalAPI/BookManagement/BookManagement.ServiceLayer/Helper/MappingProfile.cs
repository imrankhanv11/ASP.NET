using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BookManagement.DataAccessLayer.Models;
using BookManagement.ServiceLayer.DTO.Books.Request;
using BookManagement.ServiceLayer.DTO.Books.Response;
using BookManagement.ServiceLayer.DTO.Category.Request;
using BookManagement.ServiceLayer.DTO.Category.Response;

namespace BookManagement.ServiceLayer.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, GetAllBooksDTO>();

            CreateMap<Category, GetAllCategoryDTO>();

            CreateMap<Book, GetOneBookDTO>();

            CreateMap<Category, GetOneCatDTO>();

            CreateMap<AddBookDTO, Book>();

            CreateMap<AddCatDto, Category>();

            CreateMap<Book, GetOneBookByNameDTO>()
                .ForMember(d => d.CategoryName, option => option.MapFrom(src => src.Category.CategoryName));

            CreateMap<Book, CatWithProductsProductsDTO>().ReverseMap();

            CreateMap<Category, CatWithProductsDTO>();
        }
    }
}
