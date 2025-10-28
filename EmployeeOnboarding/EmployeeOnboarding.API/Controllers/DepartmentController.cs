using EmployeeOnboarding.ServiceLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeOnboarding.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {

        private readonly IDepartmentService _service;

        public DepartmentController(IDepartmentService service)
        {
            _service = service;
        }
        [HttpGet]
        [Route("department")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllDepartment()
        {
            var departments = await _service.GetAllDepartment();

            return Ok(departments);
        }

        [HttpGet]
        [Route("roles/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRoles([FromRoute] int id)
        {   
            if(id == 0 || id== null)
            {
                return BadRequest();
            }

            var roles = await _service.GetAllRole(id);

            return Ok(roles);
        }

        [HttpGet]
        [Route("location")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLocations()
        {
            var locations = await _service.GetAllLocations();

            return Ok(locations);
        }
    }
}
