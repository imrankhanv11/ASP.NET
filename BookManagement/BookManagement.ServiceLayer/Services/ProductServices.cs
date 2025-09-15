using AutoMapper;
using BookManagement.DataAccessLayer.Interfaces;
using BookManagement.DataAccessLayer.Models;
using BookManagement.ServiceLayer.DTO.Books.Request;
using BookManagement.ServiceLayer.DTO.Books.Response;
using BookManagement.ServiceLayer.Helper;
using BookManagement.ServiceLayer.Interfaces;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<ProductServices> _logger;
        private readonly IProductRepo _repo;
        public ProductServices(IGenericRepository<Book> Bookrepo, IMapper mapper, ILogger<ProductServices> logger, IProductRepo repo)
        {
            _Bookrep = Bookrepo;
            _mapper = mapper;
            _logger = logger;
            _repo = repo;
        }

        public async Task<IEnumerable<GetAllBooksDTO>> GetAllBooksService()
        {
            var value = await _repo.GetAllAsync();

            _logger.LogInformation("Fetch form DB");

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

        public async Task<GetOneBookByNameDTO> GetOnebookService(string name)
        {
            _logger.LogInformation("Enter the service");
            var value = await _repo.GetByBookName(name);

            var output = _mapper.Map<GetOneBookByNameDTO>(value);


            return output;

        }

    }
}
