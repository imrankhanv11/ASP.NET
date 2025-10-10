using ECommerce.ServiceLayer.DTO.Authentications.Request;
using ECommerce.ServiceLayer.DTO.Authentications.Response;
using ECommerce.ServiceLayer.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.APILayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService service, ILogger<AuthController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        [Route("Registration/User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDTO newUser) 
        {
            if(newUser == null)
            {
                return BadRequest("Registration can't be null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var output = await _service.RegisterUserService(newUser);

            if (!output) return BadRequest();

            return Ok(new
            {
                User = newUser.Username,
                message = "Register Successfully"
            });
        }

        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginRequestDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var output = await _service.LoginService(dto);

            if (output == null) return BadRequest();

            return Ok(output);
        }

        [HttpPost]
        [Route("Refreshtoken")]
        public async Task<ActionResult<LoginResponseDTO>> Refreshtoken([FromBody] string token) 
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest();
            }

            var output = await _service.RefreshTokenService(token);

            if (output == null) return BadRequest();

            return Ok(output);
        }
    }
}
