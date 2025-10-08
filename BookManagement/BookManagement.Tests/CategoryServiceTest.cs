using AutoMapper;
using Bogus;
using BookManagement.DataAccessLayer.Interfaces;
using BookManagement.DataAccessLayer.Models;
using BookManagement.ServiceLayer.DTO.Books.Response;
using BookManagement.ServiceLayer.DTO.Category.Request;
using BookManagement.ServiceLayer.DTO.Category.Response;
using BookManagement.ServiceLayer.Services;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.Tests
{
    public class CategoryServiceTest
    {
        private readonly Mock<IGenericRepository<Category>> _catRepo;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ICatRepo> _repo;
        private readonly CategoryService _service;

        public CategoryServiceTest()
        {
            _catRepo = new Mock<IGenericRepository<Category>>();
            _mapper = new Mock<IMapper>();
            _repo = new Mock<ICatRepo>();

            _service = new CategoryService(
                _catRepo.Object, _mapper.Object, _repo.Object
                );
        }
        
        [Fact]
        public async Task GetAllCatService_ShouldReturnCatList_WhenCatExits()
        {
            //Arrange
            var fakeCatList = new Faker<Category>()
                .RuleFor(s=> s.CategoryId, f=> f.IndexFaker + 1)
                .RuleFor(s=> s.CategoryName, f=> f.Lorem.Sentence(2,5))
                .Generate(6);

            _catRepo.Setup(s => s.GetAllAsync()).ReturnsAsync(fakeCatList);

            _mapper.Setup(s => s.Map<IEnumerable<GetAllCategoryDTO>>(fakeCatList)).Returns(fakeCatList.Select(s => new GetAllCategoryDTO
            {
                CategoryName = s.CategoryName
            }));

            // Act
            var result = await _service.GetAllCatService();

            //Assert
            result.Should().HaveCount(6);
            result.Should().NotBeNull();

            //Verify
            _catRepo.Verify(v=> v.GetAllAsync(), Times.Once());
            _mapper.Verify(v => v.Map<IEnumerable<GetAllCategoryDTO>>(It.IsAny<IEnumerable<Category>>()), Times.Once);
        }

        [Fact]
        public async Task GetOneCatService_ShouldReturnCat_WhenCatExits()
        {
            // Arrange
            var fake = new Faker<Category>()
                .RuleFor(s=> s.CategoryName, f=> f.Lorem.Sentence(2,5))
                .Generate();
            _catRepo.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(fake);

            _mapper.Setup(s => s.Map<GetOneCatDTO>(fake)).Returns(new GetOneCatDTO
            {
                CategoryName = fake.CategoryName
            });

            // Act
            var result = await _service.GetOneCatService(1);

            // Assert
            result.Should().NotBeNull();

            // Verify
            _catRepo.Verify(v => v.GetByIdAsync(It.IsAny<int>()), Times.Once());
            _mapper.Verify(v => v.Map<GetOneCatDTO>(It.IsAny<Category>()), Times.Once());
        }

        [Fact]
        public async Task GetOneCatService_ShouldThrowError_WhenCatNotExits()
        {
            _catRepo.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((Category)null);

            // Act
            var act = async () => await _service.GetOneCatService(1);

            // Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(act);

            // Verify
            _catRepo.Verify(v => v.GetByIdAsync(It.IsAny<int>()), Times.Once());
        }

        [Fact]
        public async Task AddNewCatService_ShouldReturnId_WhenCatAdded()
        {
            // Arrange
            var dto = new AddCatDto
            {
                CategoryName = "imran"
            };

            var fake = new Category
            {
                CategoryId = 1,
                CategoryName = dto.CategoryName
            };

            _mapper.Setup(s=> s.Map<Category>(dto)).Returns(fake);

            _catRepo.Setup(s=> s.AddAsync(fake)).ReturnsAsync(fake);

            // act
            var result = await _service.AddNewCatService(dto);

            result.Should().Be(1);
            result.Should().NotBe(2);
        }

        [Fact]
        public async Task DeleteCatService_ShouldReturnTrue_WhenCatDeleted()
        {
            var fake = new Faker<Category>().RuleFor(s => s.CategoryId, f => 1).Generate();
            _catRepo.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(fake);

            _catRepo.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

            var result = await _service.DeleteCatService(1);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteCatService_ShouldReturnFalse_WhenCatNotExits()
        {
            _catRepo.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((Category)null);

            var result = await _service.DeleteCatService(1);

            result.Should().BeFalse();
        }

        //public async Task<IEnumerable<CatWithProductsDTO>> catWithProductService(int id)
        //{
        //    var value = await _repo.catRepo(id);

        //    var output = _mapper.Map<IEnumerable<CatWithProductsDTO>>(value);

        //    return output;
        //}

        [Fact]
        public async Task catWithProduct_ShouldReturnCategoryWithBooks_WhenCategoryExists()
        {
            // Arrange
            int categoryId = 1;

            // Fake category with books
            var fakeCategory = new Faker<Category>()
                .RuleFor(c => c.CategoryId, categoryId)
                .RuleFor(c => c.CategoryName, f => f.Commerce.Categories(1).First())
                .RuleFor(c => c.Books, f => new List<Book>
                {
            new Book
            {
                BookId = 1,
                Title = f.Commerce.ProductName(),
                Author = f.Name.FullName(),
                Price = f.Random.Decimal(100, 500),
                Stock = f.Random.Int(5, 20),
                CategoryId = categoryId
            },
            new Book
            {
                BookId = 2,
                Title = f.Commerce.ProductName(),
                Author = f.Name.FullName(),
                Price = f.Random.Decimal(100, 500),
                Stock = f.Random.Int(5, 20),
                CategoryId = categoryId
            }
                })
                .Generate();

            // Mock repository: return a list containing one category
            _repo.Setup(r => r.catRepo(categoryId))
                     .ReturnsAsync(new List<Category> { fakeCategory });

            // Expected DTO after mapping
            var expectedDto = new List<CatWithProductsDTO>
            {
                new CatWithProductsDTO
                {
                    CategoryName = fakeCategory.CategoryName,
                    Books = fakeCategory.Books.Select(b => new CatWithProductsProductsDTO
                    {
                        Title = b.Title,
                        Author = b.Author,
                        Price = b.Price,
                        Stock = b.Stock
                    }).ToList()
                }
            };

            // Mock mapper behavior
            _mapper.Setup(m => m.Map<IEnumerable<CatWithProductsDTO>>(It.IsAny<IEnumerable<Category>>()))
                       .Returns(expectedDto);

            // Act
            var result = await _service.catWithProductService(categoryId);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().CategoryName.Should().Be(fakeCategory.CategoryName);
            result.First().Books.Should().HaveCount(2);
            result.First().Books.First().Title.Should().Be(fakeCategory.Books.First().Title);

            // Verify repo & mapper usage
            _repo.Verify(r => r.catRepo(categoryId), Times.Once);
            _mapper.Verify(m => m.Map<IEnumerable<CatWithProductsDTO>>(It.IsAny<IEnumerable<Category>>()), Times.Once);
        }


    }

}
