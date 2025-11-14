using BookManagement.ServiceLayer.DTO.Login.Request;
using BookManagement.ServiceLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace BookManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly ILoginService _service;
        public LoginController(ILoginService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("RegisterUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDTO newUser)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _service.RegisterUserService(newUser.UserName, newUser.Password);

            if (!result)
            {
                return StatusCode(500, new { message = "Registration Failed" });
            }

            return Created();
        }

        [HttpPost]
        [Route("LoginUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LoginUser([FromBody] LoginReqDTO loginUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var value = await _service.LoginUserService(loginUser);

            return Ok(value);
        }

        [HttpPost]
        [Route("RefreshToken")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDTO token)
        {
            var tokenNew = await _service.RefreshTokenService(token.RefreshToken);

            if (tokenNew != null)
            {
                return Ok(tokenNew);
            }

            return Unauthorized(new { Message = "You need to login" });
        }
    }
}
