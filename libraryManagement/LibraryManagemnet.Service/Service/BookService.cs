using AutoMapper;
using Azure.Core;
using LibararyManagement.Data.Interface;
using LibararyManagement.Data.Models;
using LibararyManagement.Data.Repository;
using LibraryManagement.Service.DTO.Account.Book.Request;
using LibraryManagement.Service.DTO.Account.Book.Responst;
using LibraryManagement.Service.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;



namespace LibraryManagement.Service.Service
{
    public class BookService : IBookService
    {
        private readonly IBookRepo _repo;
        private readonly IMapper _mapper;
        private readonly IBorrowRepo _borrowRepo;

        public BookService(IBookRepo repo, IMapper mapper, IBorrowRepo borrowRepo)
        {
            _repo = repo;
            _mapper = mapper;
            _borrowRepo = borrowRepo;
        }

        public async Task<BookAddResponseDTO> AddBookService(BookAddRequestDTO dto, string pictureLink)
        {
            var existingName = await _repo.GetBookByName(dto.BookName);

            if(existingName != null)
            {
                throw new ValidationException("Book Name Already Exits");
            }

            var newBook = new Book
            {
                BookName = dto.BookName,
                Author = dto.Author,
                Price = dto.Price,
                Stock = dto.Stock,
                CategoryId = dto.CategoryId,
                PictureLink = pictureLink
            };

            var book = await _repo.AddBookRepo(newBook);

            return new BookAddResponseDTO
            {
                Id = newBook.Id,
                BookName = newBook.BookName,
                Author = newBook.Author,
                Price = newBook.Price,
                Stock = newBook.Stock,
                Category = newBook.CategoryId,
                PictureLink = pictureLink
            };
        }

        public async Task<IEnumerable<BookGetAllResponseDTO>> GetAllService()
        {
            var result = await _repo.GetAllRepo();

            var books = result
                    .Select(s => new BookGetAllResponseDTO
                    {
                        BookName = s.BookName,
                        Id = s.Id,
                        Stock = s.Stock,
                        Price = s.Price,
                        Category = s.Category.Name,
                        PictureLink = s.PictureLink,
                        CategoryId= s.CategoryId,
                        Author = s.Author
                    })
                    .ToList();

            return books;
        }

        public async Task<IEnumerable<CatGetAllResponseDTO>> GetAllServiceCat()
        {
            var resrult = await _repo.GetAllCatRepo();

            var catList = resrult.Select(s => new CatGetAllResponseDTO
            {
                Id = s.Id,
                Name = s.Name,
            }).ToList();

            return catList;
        }

        public async Task<string> GetOneBookByID(int id)
        {
            var book = await _repo.GetoneBookByIdRepo(id);

            if (book == null)
            {
                throw new KeyNotFoundException("Book was not Found");
            }

            return book.PictureLink;
        }

        public async Task DeleteBookService(int id)
        {
            var book = await _borrowRepo.BookNotDelted(id);

            if(book != null)
            {
                throw new ValidationException("Some people Borrwed this book");
            }

            await _repo.DeleteBookRepo(id);
        }

        public async Task<BookAddResponseDTO> UpdateBookService(int id, BookAddRequestDTO dto, string newImageLink)
        {

            var book = await _repo.GetoneBookByIdRepo(id);

            book.PictureLink = newImageLink;
            book.BookName = dto.BookName;
            book.Author = dto.Author;
            book.Price = dto.Price;
            book.Stock = dto.Stock;
            book.CategoryId = dto.CategoryId;

            var newbook = await _repo.UpdateBookRepo(book);

            var updateNewBookDTO = new BookAddResponseDTO
            {
                Id = newbook.Id,
                BookName = newbook.BookName,
                Author = newbook.Author,
                Price = newbook.Price,
                Stock = newbook.Stock,
                Category = newbook.CategoryId,
                CategoryId = newbook.CategoryId,
                PictureLink = newbook.PictureLink,
            };

            return updateNewBookDTO;
        }
    }
}
