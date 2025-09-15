using BookManagement.ServiceLayer.DTO.Books.Response;
using BookManagement.ServiceLayer.DTO.Books.Request;
using BookManagement.ServiceLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IProductServices _services;

        // Logging
        private readonly ILogger<BooksController> _logger;

        public BooksController(IProductServices services, ILogger<BooksController> logger)
        {
            _services = services;
            _logger = logger;
        }

        [HttpGet]
        [Route("GellAllBooks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GetAllBooksDTO>>> GetAllBooks()
        {
            _logger.LogInformation("Enter the Method");
            var result = await _services.GetAllBooksService();
            _logger.LogInformation("Get the Book Details");

            if (result == null || !result.Any())
            {
                _logger.LogWarning("No Books Found");
                return Ok(new List<object>());
            }

            _logger.LogInformation("Books Found send results");

            return Ok(result);
        }

        [HttpGet]
        [Route("GetByID/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetOneBookDTO>> GetOneBookByID([FromRoute] int id)
        {
            var value = await _services.GetOneBookService(id);

            if(value == null)
            {
                return NotFound();
            }

            return Ok(value);
        }

        [HttpPost]
        [Route("Addbook")]
        [Authorize(Policy = "SPAdminAndAdmin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostBook([FromBody] AddBookDTO dto)
        {
            if (dto == null)
                return BadRequest("Book data is required");

            var id = await _services.AddBookService(dto);

            if (id <= 0)
                return StatusCode(StatusCodes.Status500InternalServerError, "Book could not be created");

            return CreatedAtAction(
                nameof(GetOneBookByID),
                new { idnumber = id },
                dto
            );
        }

        [HttpDelete]
        [Route("DeleteBook/{id:int}")]
        [Authorize(Policy = "SPAdminAndAdmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var value = await _services.DeleteBookService(id);

            if (!value)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpGet]
        [Route("GetBookByName/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetOneBookByNameDTO>> GetByBookName(string name)
        {
            _logger.LogInformation("Enter the controler");
            var output = await _services.GetOnebookService(name);

            _logger.LogInformation($"{name}, this one");

            if(output == null)
            {
                return NotFound();
            }

            return Ok(output);
        }
    }
}
