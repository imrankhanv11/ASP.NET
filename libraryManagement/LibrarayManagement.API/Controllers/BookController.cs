using Azure.Core;
using LibraryManagement.Service.DTO.Account.Book.Request;
using LibraryManagement.Service.DTO.Account.Book.Responst;
using LibraryManagement.Service.Interface;
using LibraryManagement.Service.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibrarayManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _service;
        private readonly IWebHostEnvironment _env;


        public BookController(IBookService service, IWebHostEnvironment env)
        {
            _service = service;
            _env = env;
        }

        [HttpPost]
        [Route("book/add")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> AddBook([FromForm] BookAddRequestDTO dto)
        {
            if (dto == null)
                return BadRequest("Invalid request");

            var webRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var folderPath = Path.Combine(webRoot, "images");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string? pictureLink = null;

            if (dto.PictureFile != null && dto.PictureFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.PictureFile.FileName);
                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.PictureFile.CopyToAsync(stream);
                }

                pictureLink = $"/images/{fileName}";
            }

            dto.PictureFile = null;
            var book = await _service.AddBookService(dto, pictureLink);

            return Ok(book);
        }

        [HttpGet]
        [Route("book/getall")]
        public async Task<ActionResult<IEnumerable<BookGetAllResponseDTO>>> GetAllBook()
        {
            var result = await _service.GetAllService();

            return Ok(result);
        }

        [HttpGet]
        [Route("Book/Cat")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetAllCat()
        {
           var result = await _service.GetAllServiceCat();

            if(result == null)
            {
                return NotFound();
            }

            return Ok(result);

        }

        [HttpDelete]
        [Route("delete/{id:int}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteBook([FromRoute] int id)
        {
            if(id == 0 || id == null)
            {
                return BadRequest();
            }


            var pictureLink = await _service.GetOneBookByID(id);

            if (!string.IsNullOrEmpty(pictureLink))
            {
                if (pictureLink.StartsWith("/"))
                    pictureLink = pictureLink.TrimStart('/');

                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", pictureLink);

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
                else
                {
                    Console.WriteLine($"File not found at: {imagePath}");
                }
            }

            await _service.DeleteBookService(id);

            return Ok(new { id = id });
        }

        [HttpPut]
        [Route("update/{id:int}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> UpdateBook([FromRoute] int id, [FromForm] BookAddRequestDTO dto)
        {
            var pictureLink = await _service.GetOneBookByID(id);

            string? newPictureLink = pictureLink;

            if (dto.PictureFile != null && dto.PictureFile.Length > 0)
            {
                var webRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var folderPath = Path.Combine(webRoot, "images");

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                if (!string.IsNullOrEmpty(pictureLink))
                {
                    var oldPath = Path.Combine(webRoot, pictureLink.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.PictureFile.FileName);
                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.PictureFile.CopyToAsync(stream);
                }

                newPictureLink = $"/images/{fileName}";
            }

            dto.PictureFile = null;

            var newBook = await _service.UpdateBookService(id, dto, newPictureLink);

            return Ok(newBook);

        }
    }
}
