using AutoMapper;
using LibararyManagement.Data.Models;
using LibraryManagement.Service.DTO.Account.Book.Responst;
using LibraryManagement.Service.DTO.Account.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Service.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserRegisterDTO, User>();

            CreateMap<IEnumerable<Book>, IEnumerable<BookGetAllResponseDTO>>();
        }
    }
}
