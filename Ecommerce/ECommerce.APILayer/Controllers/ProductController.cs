using ECommerce.ServiceLayer.DTO.Products.Request;
using ECommerce.ServiceLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.APILayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("CreateProduct")]
        [Authorize(Policy = "RequireAny_SPAdmin_Or_Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddNewProduct([FromBody] AddNewProductDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var value = await _service.AddNewProductService(dto);
            
            if(value == null || value <= 0)
            {
                return BadRequest();
            }

            return Ok(value);
        }
    }
}
