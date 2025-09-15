using BookManagement.ServiceLayer.DTO.Category.Request;
using BookManagement.ServiceLayer.DTO.Category.Response;
using BookManagement.ServiceLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace BookManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategorysController : ControllerBase
    {
        private readonly ICategoryService _sevice;
        public CategorysController(ICategoryService sevice)
        {
            _sevice = sevice;
        }

        [HttpGet]
        [Route("GetAllCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GetAllCategoryDTO>>> GetAllCat()
        {
            var value = await _sevice.GetAllCatService();

            if(value ==  null)
            {
                return Ok(new List<object>());
            }

            return Ok(value);  
        }

        [HttpGet]
        [Route("GetByID/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOneCat(int id)
        {
            var value = await _sevice.GetOneCatService(id);

            if(value == null)
            {
                return NotFound("Cat not found");
            }

            return Ok(value);
        }

        [HttpPost]
        [Route("AddNewCategory")]
        [Authorize(Policy = "SPAdminAndAdmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddNewCat([FromBody] AddCatDto dto)
        {
            var value = await _sevice.AddNewCatService(dto);

            if(value <= 0 || value == null)
            {
                return StatusCode(500, "Not added");
            }

            return Ok(value);
        }

        [HttpDelete]
        [Route("DeleteCategory/{id:int}")]
        [Authorize(Policy = "SPAdminAndAdmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCat(int id)
        {
            var value = await _sevice.DeleteCatService(id);

            if (!value)
            {
                return NotFound();
            }

            return Ok(value);
        }


    }
}
