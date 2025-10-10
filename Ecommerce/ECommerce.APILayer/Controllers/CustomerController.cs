using ECommerce.ServiceLayer.DTO.Customers.Request;
using ECommerce.ServiceLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.APILayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {

        private readonly ICustomerService _service;

        public CustomerController(ICustomerService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("OrderProducts")]
        [Authorize(Policy = "UsersOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> OrdersDetails([FromBody] OrderRequestDTO dto)
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var UID = int.Parse(id);

            if(dto == null)
            {
                return BadRequest(new
                {
                    message = "Order Details can't be empty"
                });
            }


            var details = await _service.OrderProductsService(dto,UID);

            if (!details) return BadRequest();

            return Ok();
        }

        [HttpPost]
        [Route("Addtocart")]
        [Authorize(Policy ="UsersOnly")]
        public async Task<IActionResult> AddtoCart([FromBody] CartAddDTO dto)
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            int UId = int.Parse(id);

            var output = await _service.AddtoCartService(dto, UId);

            if (!output) return BadRequest();

            return Ok();
        }
    }
}
