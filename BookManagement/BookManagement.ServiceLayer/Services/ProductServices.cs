using AutoMapper;
using BookManagement.DataAccessLayer.Interfaces;
using BookManagement.DataAccessLayer.Models;
using BookManagement.ServiceLayer.DTO.Books.Request;
using BookManagement.ServiceLayer.DTO.Books.Response;
using BookManagement.ServiceLayer.Helper;
using BookManagement.ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.ServiceLayer.Services
{
    public class ProductServices : IProductServices
    {
        private readonly IGenericRepository<Book> _Bookrep;
        private readonly IMapper _mapper;

        public ProductServices(IGenericRepository<Book> Bookrepo, IMapper mapper)
        {
            _Bookrep = Bookrepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetAllBooksDTO>> GetAllBooksService()
        {
            var value = await _Bookrep.GetAllAsync();

            var output = _mapper.Map<IEnumerable<GetAllBooksDTO>>(value);

            return output;
        }

        public async Task<GetOneBookDTO> GetOneBookService(int id)
        {
            var value = await _Bookrep.GetByIdAsync(id);

            var output = _mapper.Map<GetOneBookDTO>(value);

            return output;
        }

        public async Task<int> AddBookService(AddBookDTO book)
        {
            var value = _mapper.Map<Book>(book);

            var output = await _Bookrep.AddAsync(value);

            return output.BookId;
        }

        public async Task<bool> DeleteBookService(int id)
        {
            var value = await _Bookrep.GetByIdAsync(id);

            if(value != null)
            {
                return false;
            }

            await _Bookrep.DeleteAsync(id);

            return true;
        }

    }
}
