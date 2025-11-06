using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopSphere.ServiceLayer.DTO.Products.Request;
using ShopSphere.ServiceLayer.DTO.Products.Response;
using ShopSphere.ServiceLayer.Interface;

namespace ShopSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _service;

        public ProductController(ILogger<ProductController> logger, IProductService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpPost]
        [Route("Admin/AddProduct")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AddNewProduct([FromBody] AddProductDTO dto)
        {
            if(!ModelState.IsValid)
            {

                _logger.LogError("ModelState Error User Input Fail");
                return BadRequest(ModelState);
            }

            var result = await _service.AddNewProductService(dto);
            _logger.LogInformation("Successfully Added New product");

            return Ok(new
            {
                Product = result,
                Message = "Product Added Succesfully"
            });
        }

        [HttpGet]
        [Route("GetAll/Products")]
        //[Authorize(Policy = "AdminOrUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<GetAllProductsDTO>>> GetAllProducts()
        {
            var result = await _service.GetAllService();

            _logger.LogInformation("All Products Get Succesfully");

            return Ok(result);
        }

        [HttpPatch]
        [Route("Admin/UpdateQuanity/{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateQuantity([FromRoute] int id, [FromBody] UpdateQuantityDTO dto)
        {
            if(dto.Qunatity <=0)
            {
                return BadRequest(new
                {
                    message = "Quntity can't be Negative or Zero"
                });
            }

            await _service.UpdateQuantity(id, dto.Qunatity);

            return Ok();
        }

        [HttpPut]
        [Route("Admin/UpdateProduct/{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProduct([FromRoute] int id, [FromBody] UpdateProductDTO dto )
        {
            var product = await _service.UpdateProductService(dto, id);

            if(product != null)
            {
                return Ok(product);
            }

            return BadRequest();
        }
    }
}
