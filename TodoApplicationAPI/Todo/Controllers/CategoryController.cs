using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Todo.ModelLayer.DTO.Request;
using Todo.ModelLayer.DTO.Response;
using Todo.ServiceLayer.Interface;

namespace Todo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }


        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory([FromBody] AddCatDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _service.AddCatService(dto);

                if (!result.Success)
                    return BadRequest(new { message = result.ErrorMessage, field = result.Field });

                return Created(
                        $"/api/categories/{result.Data.id}", 
                        new { id = result.Data.id, message = "Created Successfully" } 
                    );

                //return CreatedAtAction(nameof(GetCategoryById),
                //            new { id = result.Data!.id },
                //            new { id = result.Data.id, message = "Created Successfully" });

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error: {ex.Message}");
            }
        }
    }
}
