using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Todo.ModelLayer.DTO.Request;
using Todo.ModelLayer.DTO.Response;
using Todo.ModelLayer.Models;
using Todo.ServiceLayer.Interface;

namespace Todo.Controllers
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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var value = await _service.CheckPassWordUserService(loginDto);

            if (value != null)
            {
                var token = await _service.GenerateJwtToken(value.Email, value.Roll);
                
                return Ok(token);

            }

            return Unauthorized();
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshLogin([FromBody] string token)
        {
            var tokenNew = await _service.GenerateNewToken(token);

            return Ok(tokenNew);
        }
    }
}
