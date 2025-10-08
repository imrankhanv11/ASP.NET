using ECommerce.ServiceLayer.DTO.Authentications.Request;
using ECommerce.ServiceLayer.DTO.Authentications.Response;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Writers;
using ShopSphere.DataAccessLayer.Interface;
using ShopSphere.ServiceLayer.DTO.Authentication.Request;
using ShopSphere.ServiceLayer.Interface;

namespace ShopSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly IAuthService _service;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IAuthService service, ILogger<AuthenticationController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        [Route("Register/User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDTO dto)
        {
            if(!ModelState.IsValid)
            {
                _logger.LogError("ModelState Error User Input Fail");
                return BadRequest(ModelState);
            }

            var UserID = await _service.RegisterUserService(dto);
            _logger.LogInformation("User Register Sucessfully");

            return Ok(new {
                UserID = UserID,
                Message = "User Register Successfully"
            });
        }

        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginRequestDTO dto)
        {
            if(!ModelState.IsValid)
            {
                _logger.LogError("ModelState Error User Input Fail");
                return BadRequest(ModelState);
            }

            var token = await _service.LoginService(dto);
            _logger.LogInformation("Verification done Accesstoken generted succesfully");

            return Ok(token);
            
        }

        [HttpPost]
        [Route("RefreshToken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LoginResponseDTO>> RefreshToken([FromBody] string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest();
            }

            var output = await _service.RefreshTokenService(refreshToken);

            return Ok(output);
        }
    }
}
