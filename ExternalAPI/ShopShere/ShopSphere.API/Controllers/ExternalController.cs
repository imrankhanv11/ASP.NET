using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopSphere.ServiceLayer.DTO.ExternalAPI.Request;
using ShopSphere.ServiceLayer.DTO.ExternalAPI.Response;
using ShopSphere.ServiceLayer.Interface;

namespace ShopSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExternalController : ControllerBase
    {
        private readonly IExternalAPIService _externalAPIService;

        public ExternalController(IExternalAPIService externalAPIService)
        {
            _externalAPIService = externalAPIService;
        }


        [HttpGet]
        [Route("GetAllBooks")]
        public async Task<ActionResult<IEnumerable<GetAllBookResponseDTO>>> GetAllBookExternal()
        {
            var response = await _externalAPIService.GetAllBooks();

            return Ok(response);
        }

        [HttpGet]
        [Route("GetBook/{id:int}")]
        public async Task<ActionResult<GetOneBookByIdResponseDTO>> GetOneBookById([FromRoute] int id)
        {
            var response = await _externalAPIService.GetBookById(id);

            return Ok(response);
        }

        [HttpGet]
        [Route("GetBook/{name}")]
        public async Task<ActionResult<GetOneBookByNameResponseDTO>> GetBookByName([FromRoute] string name)
        {
            var response = await _externalAPIService.GetOneBookByName(name);

            return Ok(response);
        }

        [HttpDelete]
        [Route("delete/{id:int}")]
        public async Task<IActionResult> DeleteBook([FromRoute] int id)
        {
            var respose = await _externalAPIService.DeleteBookId(id);

            return Ok();
        }

        [HttpPost]
        [Route("AddBook")]
        public async Task<IActionResult> AddBook([FromBody] AddBookRequestDTO dto)
        {
            if(dto == null)
            {
                return BadRequest();
            }

            var response = await _externalAPIService.AddBook(dto);
            
            if (response == 0)
            {
                return BadRequest();
            }

            return Ok(response);
        }
    }
}
