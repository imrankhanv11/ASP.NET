using AutoMapper;
using Bogus;
using BookManagement.API.Controllers;
using BookManagement.DataAccessLayer.Data;
using BookManagement.DataAccessLayer.Interfaces;
using BookManagement.DataAccessLayer.Models;
using BookManagement.DataAccessLayer.Repositories;
using BookManagement.ServiceLayer.DTO.Books.Request;
using BookManagement.ServiceLayer.DTO.Books.Response;
using BookManagement.ServiceLayer.Interfaces;
using BookManagement.ServiceLayer.Services;
using Castle.Core.Logging;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class IntegrationTesting
    {
        private readonly BookContext _context;
        private readonly IGenericRepository<Book> _bookRepo;
        private readonly IProductRepo _productRepo;
        private readonly IProductServices _service;
        private readonly BooksController _conroller;
        private readonly IMapper _mapper;
        private readonly Faker<Book> _fakeBooksObj;

        public IntegrationTesting()
        {
            var options = new DbContextOptionsBuilder<BookContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new BookContext(options);

            var config = new MapperConfiguration(cfg =>
            {

                cfg.CreateMap<Book, GetAllBooksDTO>();

                cfg.CreateMap<Book, GetOneBookDTO>();

                cfg.CreateMap<AddBookDTO, Book>();

                cfg.CreateMap<Book, GetOneBookByNameDTO>()
                    .ForMember(d => d.CategoryName, option => option.MapFrom(src => src.Category.CategoryName));

            });

            _mapper = config.CreateMapper();

            _productRepo = new ProductRepo(_context);
            _bookRepo = new GenericRepository<Book>(_context);
            _service = new ProductServices(_bookRepo, _mapper, _productRepo);
            _conroller = new BooksController(_service);

            _fakeBooksObj = new Faker<Book>()
                .RuleFor(b => b.BookId, f => f.IndexFaker + 1)
                .RuleFor(b => b.Title, f => f.Lorem.Sentence(3, 5))
                .RuleFor(b => b.Author, f => f.Person.FullName)
                .RuleFor(b => b.Price, f => f.Random.Decimal(100, 1000))
                .RuleFor(b => b.Stock, f => f.Random.Int(1, 50))
                .RuleFor(b => b.CategoryId, f => f.Random.Int(1, 5));
        }

        // Get By ID
        [Fact]
        public async Task GetOneBookByID_ShouldReturnBook_WhenBookExits()
        {
            // Arrange
            _context.Books.RemoveRange(_context.Books);
            await _context.SaveChangesAsync();
            var fakeBooks = _fakeBooksObj.Generate(4);

            await _context.Books.AddRangeAsync(fakeBooks);
            await _context.SaveChangesAsync();

            // Act
            var result = await _conroller.GetOneBookByID(1);

            // Assert
            using (new AssertionScope())
            {
                result.Result.Should().BeOfType<OkObjectResult>();

                var okResult = result.Result as OkObjectResult;
                okResult.Should().NotBeNull();
                okResult.StatusCode.Should().Be(200);

                var dto = okResult.Value as GetOneBookDTO;
                dto.Should().NotBeNull();
                dto.Title.Should().Be(fakeBooks.First().Title);
                dto.Author.Should().Be(fakeBooks.First().Author);
            }
        }

        [Fact]
        public async Task GetOneBookByID_ShouldReturnNotFound_WhenBookNotExits()
        {
            // Arrange
            //_context.Books.RemoveRange(_context.Books);
            //await _context.SaveChangesAsync();

            // Act 
            var result = await _conroller.GetOneBookByID(1);

            // Assert
            using (new AssertionScope())
            {
                result.Result.Should().BeOfType<NotFoundObjectResult>();

                var notFoundResult = result.Result as NotFoundObjectResult;
                notFoundResult!.StatusCode.Should().Be(404); 
            }
        }

        // Get all
        [Fact]
        public async Task GetAllBooks_ShouldReturnList_WhenBooksExist()
        {
            // Arrange
            var fakeBooks = _fakeBooksObj.Generate(10);
            await _context.AddRangeAsync(fakeBooks);
            await _context.SaveChangesAsync();

            // Act
            var actionResult = await _conroller.GetAllBooks();

            // Assert
            using (new AssertionScope())
            {
                var okResult = actionResult.Result as OkObjectResult;
                okResult.Should().NotBeNull();
                okResult.StatusCode.Should().Be(StatusCodes.Status200OK);

                var dto = okResult.Value as IEnumerable<GetAllBooksDTO>;
                dto.Should().NotBeNull();
                dto!.Count().Should().Be(fakeBooks.Count);

                dto.First().Author.Should().Be(fakeBooks.First().Author);
            }
        }

        [Fact]
        public async Task GetAllBooks_ShouldReturnNoContent_WhenBooksEmpty()
        {
            // Arrange
            _context.Books.RemoveRange(_context.Books);
            await _context.SaveChangesAsync();

            // Act
            var actionResult = await _conroller.GetAllBooks();

            // Assert
            using (new AssertionScope())
            {
                actionResult.Result.Should().BeOfType<NoContentResult>();
                var noContentResult = actionResult.Result as NoContentResult;

                noContentResult!.StatusCode.Should().Be(StatusCodes.Status204NoContent);
            }
        }

        // Post
        [Fact]
        public async Task PostBook_ShouldReturnCreated_WhenBookIsValid()
        {
            // Arrange
            var newBook = new AddBookDTO
            {
                Title = "Integration Test Book",
                Author = "Test Author",
                Price = 150
            };

            // Act
            var result = await _conroller.PostBook(newBook);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeOfType<CreatedAtActionResult>();
                var createdResult = result as CreatedAtActionResult;
                createdResult.Should().NotBeNull();

                createdResult!.StatusCode.Should().Be(StatusCodes.Status201Created);

                createdResult.ActionName.Should().Be(nameof(_conroller.GetOneBookByID));

                var returnedDto = createdResult.Value as AddBookDTO;
                returnedDto.Should().NotBeNull();
                returnedDto!.Title.Should().Be(newBook.Title);
            }
        }

        [Fact]
        public async Task PostBook_ShouldReturnBadRequest_WhenDtoIsNull()
        {
            // Act
            var result = await _conroller.PostBook((AddBookDTO)null);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequest = result as BadRequestObjectResult;
            badRequest!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            badRequest.Value.Should().Be("Book data is required");
        }

        [Fact]
        public async Task PostBook_ShouldReturnInternalServerError_WhenServiceFails()
        {
            // Arrange
            var newBook = new AddBookDTO
            {
                Title = "Fail Book",
                Author = "Fail Author",
                Price = 200
            };

            // Mock the service to return 0 (failure)
            var serviceMock = new Mock<IProductServices>();
            serviceMock.Setup(s => s.AddBookService(It.IsAny<AddBookDTO>())).ReturnsAsync(0);
            var controllerWithMock = new BooksController(serviceMock.Object);

            // Act
            var result = await controllerWithMock.PostBook(newBook);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objResult = result as ObjectResult;
            objResult!.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            objResult.Value.Should().Be("Book could not be created");
        }

        // Delete
        [Fact]
        public async Task DeleteBook_ShouldDelete_WhenBookExits()
        {
            var fakeBook = _fakeBooksObj.Generate();
            _context.Add(fakeBook);
            await _context.SaveChangesAsync();

            var result = await _conroller.DeleteBook(1);

            using(new AssertionScope())
            {
                result.Should().BeOfType<OkObjectResult>();

                var okResult = result as OkObjectResult;
                okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);
                okResult.Value.Should().BeEquivalentTo(new { message = "Deleted Sucessfully" });
            }
        }

        [Fact]
        public async Task DeleteBook_ShouldThorwError_WhenBookNotExits()
        {
            //Arrange
            int id = 999;

            // Act
            var act = async () => await _conroller.DeleteBook(id);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                 .WithMessage($"Book with id {id} does not exist.");
        }

    }
}
