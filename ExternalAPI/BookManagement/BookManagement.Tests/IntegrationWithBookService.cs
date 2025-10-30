using AutoMapper;
using Bogus;
using BookManagement.DataAccessLayer.Data;
using BookManagement.DataAccessLayer.Interfaces;
using BookManagement.DataAccessLayer.Models;
using BookManagement.DataAccessLayer.Repositories;
using BookManagement.ServiceLayer.DTO.Books.Request;
using BookManagement.ServiceLayer.DTO.Books.Response;
using BookManagement.ServiceLayer.Interfaces;
using BookManagement.ServiceLayer.Services;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.Tests
{
    public class IntegrationWithBookService
    {
        private readonly IGenericRepository<Book> _Bookrep;
        private readonly IMapper _mapper;
        private readonly IProductRepo _repo;
        private readonly ProductServices _service;
        private readonly BookContext _context;

        public IntegrationWithBookService()
        {
            var options = new DbContextOptionsBuilder<BookContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new BookContext(options);

            _Bookrep = new GenericRepository<Book>(_context);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Book, GetAllBooksDTO>();
                cfg.CreateMap<Book, GetOneBookDTO>();
                cfg.CreateMap<AddBookDTO, Book>();
                cfg.CreateMap<Book, GetOneBookByNameDTO>()
                .ForMember(d => d.CategoryName, option => option.MapFrom(src => src.Category.CategoryName));
            });

            _mapper = config.CreateMapper();

            //_logger = new Mock<ILogger<ProductServices>>().Object;

            _repo = new ProductRepo(_context);

            _service = new ProductServices(_Bookrep, _mapper, _repo);
        }

        [Fact]
        public async Task GetAllBooksService_ShouldReturnList()
        {
            var fakeBooks = new Faker<Book>()
                .RuleFor(b => b.BookId, f => f.IndexFaker + 1)
                .RuleFor(b => b.Title, f => f.Lorem.Sentence(3, 5))
                .RuleFor(b => b.Author, f => f.Person.FullName)
                .RuleFor(b => b.Price, f => f.Random.Decimal(100, 1000))
                .RuleFor(b => b.Stock, f => f.Random.Int(1, 50))
                .RuleFor(b => b.CategoryId, f => f.Random.Int(1, 5))
                .Generate(5);

            _context.Books.AddRange(fakeBooks);
            await _context.SaveChangesAsync();

            var result = await _service.GetAllBooksService();

            using (new AssertionScope())
            {
                result.Should().HaveCount(5);
                result.Should().NotBeNull();
            }
        }

        [Fact]
        public async Task GetOneBookService_ShouldReturnBook()
        {
            var fakeBooks = new Faker<Book>()
                .RuleFor(b => b.BookId, f => f.IndexFaker + 1)
                .RuleFor(b => b.Title, f => f.Lorem.Sentence(3, 5))
                .RuleFor(b => b.Author, f => "imran")
                .RuleFor(b => b.Price, f => f.Random.Decimal(100, 1000))
                .RuleFor(b => b.Stock, f => f.Random.Int(1, 50))
                .RuleFor(b => b.CategoryId, f => 1)
                .Generate(5);

            _context.Books.AddRange(fakeBooks);
            await _context.SaveChangesAsync();

            var result = await _service.GetOneBookService(5);

            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Author.Should().NotBeNull();
                result.Author.Should().Be("imran");
            }
        }

        [Fact]
        public async Task AddBookService_ShouldReturnId()
        {
            var fakeBooks = new Faker<AddBookDTO>()
                .RuleFor(b => b.Title, f => f.Lorem.Sentence(3, 5))
                .RuleFor(b => b.Author, f => "imran")
                .RuleFor(b => b.Price, f => f.Random.Decimal(100, 1000))
                .RuleFor(b => b.Stock, f => f.Random.Int(1, 50))
                .RuleFor(b => b.CategoryId, f => 1)
                .Generate();

            var result = await _service.AddBookService(fakeBooks);

            var id = await _context.Books.FindAsync(result);
            
            using(new AssertionScope())
            {
                result.Should().BeGreaterThan(0);
                id.Should().NotBeNull();
                id.Author.Should().Be(fakeBooks.Author);
                id.Title.Should().Be(fakeBooks.Title);
            }

        }

        [Fact]
        public async Task DeleteBookService_ShouldReturnTrue()
        {
            // Arrange
            var fakeBooks = new Faker<Book>()
                .RuleFor(b => b.BookId, f => f.IndexFaker + 1)
                .RuleFor(b => b.Title, f => f.Lorem.Sentence(3, 5))
                .RuleFor(b => b.Author, f => "imran")
                .RuleFor(b => b.Price, f => f.Random.Decimal(100, 1000))
                .RuleFor(b => b.Stock, f => f.Random.Int(1, 50))
                .RuleFor(b => b.CategoryId, f => 1)
                .Generate(5);

            _context.Books.AddRange(fakeBooks);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.DeleteBookService(4);

            // Assert
            result.Should().BeTrue();

        }

        [Fact]
        public async Task DeleteBookService_ShouldReturnFalse()
        {
            int id = 4;
            // Act
            var result = async () => await _service.DeleteBookService(id);

            await result.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Book with id {id} does not exist.");
        }
    }
}
