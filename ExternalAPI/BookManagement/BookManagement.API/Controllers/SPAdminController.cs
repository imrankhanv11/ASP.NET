using BookManagement.ServiceLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SPAdminController : ControllerBase
    {
        private readonly ISPAdminService _spAdminService;
        private readonly ILogger<SPAdminController> _logger;

        public SPAdminController(ISPAdminService service, ILogger<SPAdminController> logger)
        {
            _logger= logger;
            _spAdminService= service;
        }

        [HttpPut]
        [Route("AddAdmin/{id:int}")]
        [Authorize(Policy = "SPAdminOnly")]
        public async Task<IActionResult> AddAdmin([FromRoute] int id)
        {
            var value = await _spAdminService.AddAdminService(id);

            if(!value)
            {
                return BadRequest();
            }

            return Ok();
            
        }
    }
}
