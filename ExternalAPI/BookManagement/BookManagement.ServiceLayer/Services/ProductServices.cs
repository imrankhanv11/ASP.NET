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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.ServiceLayer.Services
{
    public class ProductServices : IProductServices
    {
        private readonly IGenericRepository<Book> _Bookrep;
        private readonly IMapper _mapper;
        //private readonly ILogger<ProductServices> _logger;
        private readonly IProductRepo _repo;
        public ProductServices(IGenericRepository<Book> Bookrepo, IMapper mapper, IProductRepo repo)
        {
            _Bookrep = Bookrepo;
            _mapper = mapper;
            //_logger = logger;
            _repo = repo;
        }

        public async Task<IEnumerable<GetAllBooksDTO>> GetAllBooksService()
        {
            var value = await _repo.GetAllAsync();

            //_logger.LogInformation("Fetch form DB");

            var output = _mapper.Map<IEnumerable<GetAllBooksDTO>>(value);

            return output;
        }

        public async Task<GetOneBookDTO> GetOneBookService(int id)
        {
            var value = await _Bookrep.GetByIdAsync(id);

            var output = _mapper.Map<GetOneBookDTO>(value);

            return output;
        }

        public async Task<AddBookResponseDTO> AddBookService(AddBookDTO book)
        {
            if(book.Title.Trim().Length <= 2)
            {
                throw new ValidationException("Book Title Can't be Less than 3 Letter");
            }

            if (book.Author.Trim().Length <= 2)
            {
                throw new ValidationException("Book Author Name Can't be Less than 3 Letter");
            }

            var value = _mapper.Map<Book>(book);

            var output = await _Bookrep.AddAsync(value);

            return new AddBookResponseDTO
            {
                BookId = output.BookId,
                Price = output.Price,
                CategoryId = output.CategoryId,
                Author = output.Author,
                Stock = output.Stock,
                Title = output.Title,
            };
        }

        public async Task<bool> DeleteBookService(int id)
        {
            var book = await _Bookrep.GetByIdAsync(id);

            if (book == null)
            {
                throw new KeyNotFoundException($"Book with id {id} does not exist.");
            }

            await _Bookrep.DeleteAsync(id);
            return true;
        }

        public async Task<GetOneBookByNameDTO> GetOnebookService(string name)
        {
            //_logger.LogInformation("Enter the service");
            var value = await _repo.GetByBookName(name);

            if(value == null)
            {
                throw new KeyNotFoundException("Book not found");
            }

            var output = _mapper.Map<GetOneBookByNameDTO>(value);


            return output;

        }

    }
}
