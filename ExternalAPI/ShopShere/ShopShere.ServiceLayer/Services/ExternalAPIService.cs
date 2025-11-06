using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using ShopSphere.ServiceLayer.DTO.ExternalAPI.Request;
using ShopSphere.ServiceLayer.DTO.ExternalAPI.Response;
using ShopSphere.ServiceLayer.DTO.ExternalAPI.Response.ErrorResponse;
using ShopSphere.ServiceLayer.EndPoints;
using ShopSphere.ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace ShopSphere.ServiceLayer.Services
{
    public class ExternalAPIService : IExternalAPIService
    {
        private readonly HttpClient _httpClient;

        public ExternalAPIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        // Get All
        public async Task<List<GetAllBookResponseDTO>> GetAllBooks()
        {
            var response = await _httpClient.GetAsync(Enpoints.GetAllBooks);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var books = JsonSerializer.Deserialize<List<GetAllBookResponseDTO>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return books;

            }
            else if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {

                return new List<GetAllBookResponseDTO>();
            }

            return new List<GetAllBookResponseDTO>();
        }

        // get one book by id
        public async Task<GetAllBookResponseDTO?> GetBookById(int id)
        {
            var response = await _httpClient.GetAsync($"{Enpoints.GetOneBookById}/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var book = JsonSerializer.Deserialize<GetAllBookResponseDTO>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true 
                });

                return book;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                var errorContent = await response.Content.ReadAsStringAsync();

                var error = JsonSerializer.Deserialize<Dictionary<string, string>>(errorContent);

                var message = (error != null && error.ContainsKey("message")) ? error["message"] : "Book not found Second.";

                throw new KeyNotFoundException(message);
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"API Error ({response.StatusCode}): {error}");
            }
        }

        // get book by name
        public async Task<GetOneBookByNameResponseDTO?> GetOneBookByName(string name)
        {
            var response = await _httpClient.GetAsync($"{Enpoints.GetOneBookByName}/{name}");

            if(response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var book = JsonSerializer.Deserialize<GetOneBookByNameResponseDTO>(content,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );

                return book;
            }
            else if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                var errorContent = await response.Content.ReadAsStringAsync();

                var error = JsonSerializer.Deserialize<GlobalErrorResponseDTO>(errorContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                throw new KeyNotFoundException(error.Message);
            }

            return null;
        }

        // delete
        public async Task<bool> DeleteBookId(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Enpoints.DeleteBook}/{id}");

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();

                var error = JsonSerializer.Deserialize<GlobalErrorResponseDTO>(errorResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                throw new KeyNotFoundException(error.Message);
            }

            return false;
        }

        // post
        public async Task<int> AddBook(AddBookRequestDTO dto)
        {
            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(Enpoints.AddBok, content);

            if (response.IsSuccessStatusCode)
            {
                var contentResponse = await response.Content.ReadAsStringAsync();

                var newBook = JsonSerializer.Deserialize<int>(contentResponse);
                
                return newBook;
            }
            else if(response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var contentResponse = await response.Content.ReadAsStringAsync();

                var newError = JsonSerializer.Deserialize<GlobalErrorResponseDTO>(contentResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                throw new ValidationException(newError.Message?? "Validation Error");
            }

            return 0;
        }
    }
}