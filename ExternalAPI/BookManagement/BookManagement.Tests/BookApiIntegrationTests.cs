using BookManagement.DataAccessLayer.Data;
using BookManagement.DataAccessLayer.Models;
using BookManagement.ServiceLayer.DTO.Books.Request;
using BookManagement.ServiceLayer.DTO.Books.Response;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace BookManagement.Tests
{
    public class BookApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;
        


        public BookApiIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllBooks_ShouldReturnOkAndBooks()
        {
            // Act
            var response = await _client.GetAsync("/api/books/GellAllBooks");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var books = await response.Content.ReadFromJsonAsync<List<GetAllBooksDTO>>();
            books.Should().NotBeNull();
            books.Should().NotBeEmpty();
        }


        [Fact]
        public async Task GetBookById_ShouldReturnOk_WhenBookExists()
        {
            // Act
            var response = await _client.GetAsync("/api/books/GetByID/1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetBookById_ShouldReturnNotFound_WhenBookDoesNotExist()
        {
            // Act
            var response = await _client.GetAsync("/api/books/GetByID/999");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddBook_ShouldReturnCreated()
        {
            var newBookDto = new AddBookDTO
            {
                Title = "Domain-Driven Design",
                Author = "Eric Evans"
            };

            var response = await _client.PostAsJsonAsync("/api/books/Addbook", newBookDto);

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var returnedBook = await response.Content.ReadFromJsonAsync<AddBookDTO>();
            returnedBook!.Title.Should().Be("Domain-Driven Design");
        }

        [Fact]
        public async Task DeleteBook_ShouldReturnOk_WhenBookExists()
        {
            // Arrange: ensure book exists
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<BookContext>();
                context.Books.Add(new Book { BookId = 100, Title = "To Delete", Author = "Author A" });
                await context.SaveChangesAsync();
            }

            // Act
            var response = await _client.DeleteAsync("/api/books/DeleteBook/100");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var message = await response.Content.ReadFromJsonAsync<dynamic>();
            Assert.Equal("Deleted Sucessfully", (string)message.message);
        }

        //[Fact]
        //public async Task DeleteBook_ShouldReturnBadRequest_WhenBookDoesNotExist()
        //{
        //    var response = await _client.DeleteAsync("/api/books/DeleteBook/9999");
        //    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        //}

    }
}
