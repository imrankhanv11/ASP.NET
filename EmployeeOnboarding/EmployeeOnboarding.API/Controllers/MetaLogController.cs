using EmployeeOnboarding.ServiceLayer.DTO.MetaLog.Request;
using EmployeeOnboarding.ServiceLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeOnboarding.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetaLogController : ControllerBase
    {
        private readonly IMetaLogService _service;

        public MetaLogController(IMetaLogService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("metaLog")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> MetaLogDepartment([FromBody] MetaLogRequestDTO dto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var metaLog = await _service.MetaLogServiceCall(dto);

            return Ok(metaLog);
        }
    }
}
