using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopSphere.ServiceLayer.DTO.Cart.Request;
using ShopSphere.ServiceLayer.DTO.Cart.Response;
using ShopSphere.ServiceLayer.Interface;
using System.Net.NetworkInformation;
using System.Security.Claims;

namespace ShopSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ILogger<CartController> _logger;
        private readonly ICartService _service;

        public CartController(ILogger<CartController> logger, ICartService service)
        {
            _logger = logger;
            _service = service;
        }


        [HttpPost]
        [Route("AddtoCart")]
        [Authorize("UsersOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AddtoCart([FromBody] AddCartItemsDTO dto)
        {

            if (!ModelState.IsValid)
            {

                _logger.LogError("ModelState Error User Input Fail");
                return BadRequest();
            }

            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var UserId = int.Parse(id);
            _logger.LogInformation("Specific UserID found");

            var result = await _service.AddtoCartService(dto, UserId);
            _logger.LogInformation("Succesfully Card Addeded");

            return Ok(result);

        }

        [HttpGet]
        [Route("GetCartItems")]
        [Authorize("UsersOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<GetAllCart>>> GetAllCartItems()
        {

            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var UserId = int.Parse(id);
            _logger.LogInformation("Specific UserID found");


            var ouptut = await _service.GetAllCartService(UserId);
            _logger.LogInformation("Cart Items Getting successfully");

            var Final = await _service.GetTotalAmount(ouptut);
            _logger.LogInformation("Cart Item's total amount Calculated");

            return Ok(new
            {
                Result = ouptut,
                TotalAmount = Final
            });
        }

        [HttpPut("IncreaseQuantity/{cartItemId:int}")]
        [Authorize(Policy = "UsersOnly")]
        public async Task<IActionResult> IncreaseQuantity([FromRoute] int cartItemId)
        {
            var updatedCart = await _service.IncreaseQuantity(cartItemId);
            if (updatedCart == null)
                return NotFound();
            return Ok(updatedCart);
        }

        [HttpPut("DecreaseQuantity/{cartItemId:int}")]
        [Authorize(Policy = "UsersOnly")]
        public async Task<IActionResult> DecreaseQuantity([FromRoute] int cartItemId)
        {
            var updatedCart = await _service.DecreaseQuantity(cartItemId);
            if (updatedCart == null)
                return NotFound();
            return Ok(updatedCart);
        }


        [HttpDelete]
        [Authorize("UsersOnly")]
        [Route("RemoveCartItem")]
        public async Task<IActionResult> DeleteCart([FromBody] CartDeleteRequest dto)
        {
            var result = await _service.cartDeleteDTO(dto);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
