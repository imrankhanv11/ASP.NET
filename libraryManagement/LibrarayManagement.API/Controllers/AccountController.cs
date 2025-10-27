using LibraryManagement.Service.DTO.Account.Request;
using LibraryManagement.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibrarayManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _service;
        public AccountController(IAccountService service)
        {
            _service = service; 
        }

        [HttpPost]
        [Route("user/register")]
        public async Task<IActionResult> UserRegister([FromBody] UserRegisterDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _service.UserRegisterService(dto);

            if (user == null)
            {
                return BadRequest(new { message = "Registeration Failed"});
            }

            return Ok(user);
        }

        [HttpPost]
        [Route("user/login")]
        public async Task<IActionResult> UserLogin([FromBody] UserLoginRequestDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tokes = await _service.UserLoginService(dto);   

            if(tokes == null)
            {
                return BadRequest(new { message = "Login Failed" });
            }

            return Ok(tokes);   
        }

        [HttpPost]
        [Route("user/refreshtoken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDTO dto)
        {
            if (string.IsNullOrEmpty(dto.Token))
            {
                return BadRequest();
            }

            var output = await _service.RefreshTokenService(dto.Token);

            return Ok(output);
        }
    }
}
