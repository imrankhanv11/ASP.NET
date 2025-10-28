using EmployeeOnboarding.ServiceLayer.DTO.Employee.Request;
using EmployeeOnboarding.ServiceLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeOnboarding.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _service;

        public EmployeeController(IEmployeeService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("onboardEmployee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> OnboardEmployee([FromBody] EmployeeOnboardRequestDTO newEmpDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var onBoardedEmployee = await _service.EmployeeOnboardingService(newEmpDto);

            return Ok(onBoardedEmployee);
        }
    }
}
