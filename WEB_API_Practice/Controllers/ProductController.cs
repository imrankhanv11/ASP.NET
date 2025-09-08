using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Identity.Client;
using System.Collections.Specialized;
using Web_API_Practice.DTO;
using Web_API_Practice.Interfaces;

namespace Web_API_Practice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AllProductsDTO>>> GellAll()
        {
            var products = await _service.GetAllProducts();

            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult> AddProduct([FromBody] AddProduct dto)
        {
            await _service.AddProduct(dto);
            return Ok(new
            {
                message = "product add succesfully"
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductWithID(int id)
        {
            try
            {
                var success = await _service.DeleteProductService(id);

                if (!success) return NotFound();

                return NoContent();
            }
            catch
            {
                return StatusCode(500, "An error occurred while deleting the product");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AllProductsDTO>> GetProductWithID(int id)
        {
            try
            {
                var product = await _service.GetProductWithIDService(id);

                if (product == null)
                    return NotFound();

                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving the product");
            }
        }
    }
}
