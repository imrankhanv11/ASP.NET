using AutoMapper;
using Bogus;
using BookManagement.DataAccessLayer.Interfaces;
using BookManagement.DataAccessLayer.Models;
using BookManagement.ServiceLayer.DTO.Books.Request;
using BookManagement.ServiceLayer.DTO.Books.Response;
using BookManagement.ServiceLayer.Services;
using Castle.Core.Logging;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace BookManagement.Tests
{
    public class BookServiceTest
    {

        private readonly Mock<IGenericRepository<Book>> _mockGenericRepo;
        private readonly Mock<IProductRepo> _mockProductRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<ProductServices>> _mockLogger;
        private readonly ProductServices _service;

        public BookServiceTest()
        {
            // Initialize mocks
            _mockGenericRepo = new Mock<IGenericRepository<Book>>();
            _mockProductRepo = new Mock<IProductRepo>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<ProductServices>>();

            // Create service instance with mocks
            _service = new ProductServices(
                _mockGenericRepo.Object,
                _mockMapper.Object,
                _mockLogger.Object,
                _mockProductRepo.Object
            );
        }

        // Get by Id
        [Fact]
        public async Task GetBookById_ShouldReturnBook_WhenBookExists()
        {


            // Arrange
            var fakeBook = new Faker<Book>()
                .RuleFor(b => b.BookId, f => 2)
                .RuleFor(b => b.Title, f => f.Lorem.Sentence(3, 5))
                .RuleFor(b => b.Author, f => f.Person.FullName)
                .RuleFor(b => b.Price, f => f.Random.Decimal(100, 1000))
                .RuleFor(b => b.Stock, f => f.Random.Int(1, 50))
                .RuleFor(b => b.CategoryId, f => f.Random.Int(1, 5))
                .Generate();

            _mockGenericRepo
                .Setup(r => r.GetByIdAsync(2))
                .ReturnsAsync(fakeBook);

            // Auto mapper
            _mockMapper
                .Setup(m => m.Map<GetOneBookDTO>(fakeBook))
                .Returns(new GetOneBookDTO
                {
                    Title = fakeBook.Title,
                    Author = fakeBook.Author,
                    Price = fakeBook.Price,
                    Stock = fakeBook.Stock,
                    CategoryId = fakeBook.CategoryId
                });

            // Act
            var result = await _service.GetOneBookService(2);

            // Assert
            result.Should().NotBeNull();
            result.Title.Should().Be(fakeBook.Title);
            result.Author.Should().Be(fakeBook.Author);
        }

        // Get all
        [Fact]
        public async Task GetAllBooks_ShouldReturnList_WhenBooksExist()
        {
            // Arrange
            var fakeBooks = new Faker<Book>()
                .RuleFor(b => b.BookId, f => 1)
                .RuleFor(b => b.Title, f => f.Lorem.Sentence(3, 5))
                .RuleFor(b => b.Author, f => f.Person.FullName)
                .RuleFor(b => b.Price, f => f.Random.Decimal(100, 1000))
                .RuleFor(b => b.Stock, f => f.Random.Int(1, 50))
                .RuleFor(b => b.CategoryId, f => f.Random.Int(1, 5))
                .Generate(4);

            _mockProductRepo
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(fakeBooks);

            _mockMapper
                .Setup(m => m.Map<IEnumerable<GetAllBooksDTO>>(fakeBooks))
                .Returns(fakeBooks.Select(b => new GetAllBooksDTO
                {
                    Title = b.Title,
                    Author = b.Author,
                    Price = b.Price,
                    Stock = b.Stock,
                    CategoryId = b.CategoryId
                }));

            // Act
            var result = await _service.GetAllBooksService();

            // Assert
            result.Should().HaveCount(4);
            result.First().Title.Should().Be(fakeBooks.First().Title);
        }

        // add 
        [Fact]
        public async Task AddBookService_ShouldReturnId_WhenBookAdded()
        {
            // Arrange
            var dto = new AddBookDTO
            {
                Author = "imran",
                Title = "title",
                Price = 45,
                Stock = 4,
                CategoryId = 1
            };

            var fakeproduct = new Book
            {
                BookId = 1,
                Author = dto.Author,
                Title = dto.Title,
                Price = dto.Price,
                Stock = dto.Stock,
                CategoryId = dto.CategoryId
            };

            _mockMapper.Setup(m => m.Map<Book>(dto)).Returns(fakeproduct);

            _mockGenericRepo.Setup(m=> m.AddAsync(fakeproduct)).ReturnsAsync(fakeproduct);

            // Act
            var result = await _service.AddBookService(dto);

            // Assert
            result.Should().Be(1);

            // Verify
            _mockMapper.Verify(v=> v.Map<Book>(It.IsAny<AddBookDTO>()), Times.Once);
            _mockGenericRepo.Verify(v => v.AddAsync(It.IsAny<Book>()), Times.Once);
        }

        [Fact]
        public async Task DeleteBookService_ShouldDelete_WhenBookExists()
        {
            // Arrange
            var fakeBook = new Faker<Book>()
                .RuleFor(b => b.BookId, f => 1)
                .Generate();

            _mockGenericRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(fakeBook);
            _mockGenericRepo.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _service.DeleteBookService(1);

            // Assert
            result.Should().BeTrue();
            _mockGenericRepo.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteBookService_ShouldThrowException_WhenBookDoesNotExist()
        {
            // Arrange
            _mockGenericRepo.Setup(r => r.GetByIdAsync(2)).ReturnsAsync((Book)null);

            // Act & Assert
            //await Assert.ThrowsAsync<InvalidOperationException>(() => _service.DeleteBookService(2));

            // Act
            var act = async () => await _service.DeleteBookService(2);

            // Asser
            await Assert.ThrowsAsync<InvalidOperationException>(act);

            // verify
            _mockGenericRepo.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetOneBookService_ShuldReturnBook_WhenBookExitst()
        {
            // Arrange
            string book = "imran";
            var fakeBook = new Faker<Book>()
                .RuleFor(s=> s.Author, f=> f.Lorem.Sentence(2,3))
                .RuleFor(s=> s.Title, f=> f.Lorem.Sentence(3,2))
                .RuleFor(b => b.Price, f => f.Random.Decimal(100, 1000))
                .RuleFor(b => b.Stock, f => f.Random.Int(1, 50))
                .RuleFor(b => b.Category, f => new Category { CategoryName = f.Lorem.Word() })
                .Generate();
            _mockProductRepo.Setup(s => s.GetByBookName(book)).ReturnsAsync(fakeBook);

            // Automapper 
            _mockMapper.Setup(s => s.Map<GetOneBookByNameDTO>(fakeBook))
                .Returns(new GetOneBookByNameDTO
                {
                    Author = fakeBook.Author,
                    Title = fakeBook.Title,
                    Price = fakeBook.Price,
                    Stock = fakeBook.Stock,
                    CategoryName = fakeBook.Category.CategoryName,
                });

            // Act
            var result = await _service.GetOnebookService(book);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(fakeBook, options => options
            .ExcludingMissingMembers());
            result.Title.Should().Be(fakeBook.Title);

            //Verify
            _mockProductRepo.Verify(v => v.GetByBookName(It.IsAny<string>()), Times.Once);
            _mockMapper.Verify(v=> v.Map<GetOneBookByNameDTO>(It.IsAny<Book>()), Times.Once);
        }

        [Fact]
        public async Task GetOneBokService_ShouldThrowError_WhenBookNotExists()
        {
            //Arrange 
            string book = "imran";
            _mockProductRepo.Setup(s => s.GetByBookName(book))
                .ReturnsAsync((Book)null);

            //// Act
            //var act = async () => await _service.GetOnebookService(book);

            //// Assert
            //await Assert.ThrowsAsync<KeyNotFoundException>(act);


            // Act
            Func<Task> act = () => _service.GetOnebookService(book);

            // Assert using FluentAssertions
            await act.Should().ThrowAsync<KeyNotFoundException>();


            //Verify
            _mockProductRepo.Verify(v => v.GetByBookName(It.IsAny<string>()), Times.Once);
            _mockMapper.Verify(v => v.Map<GetOneBookByNameDTO>(It.IsAny<Book>()), Times.Never);
        }
    }
}
