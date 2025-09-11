using Azure.Core;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Security.Claims;
using Todo.ModelLayer.DTO.Request;
using Todo.ModelLayer.DTO.Response;
using Todo.ServiceLayer.Interface;

namespace Todo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _service;

        public TodoController(ITodoService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("AddTodo")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateTodo([FromBody] TodoAddDTO tododto)
        {
            if (tododto == null)
            {
                return BadRequest(new { Message = "Todo Not be empty"});
            }

            var UserID = User.Claims.FirstOrDefault(s=> s.Type == ClaimTypes.NameIdentifier);

            var id = int.Parse(UserID.Value);
            try
            {
                var result = await _service.TodoAddservice(tododto, id);

                if (!result.Success)
                {
                    return BadRequest(new {Message = result.ErrorMessage, Field = result.Field });
                }

                return CreatedAtAction(
                    nameof(GetOne),
                    new { id = result.Data?.Id, Message = "Created Succesfully" }
                    );
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Problem {ex.Message}");
            }
        }


        [HttpGet]
        [Route("GetAll")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<TodoGetAll>>> GetAll()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized(new { message = "Invalid Token or User not found" });
            }

            var userId = int.Parse(userIdClaim.Value);

            try
            {
                var result = await _service.GetAllService(userId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error {ex.Message}");
            }
        }


        [HttpDelete("DeleteTodobyid/{id:int}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized(new { message = "You must be logged in to perform this action." });
            }

                var UserID = User.Claims.FirstOrDefault(s=> s.Type == ClaimTypes.NameIdentifier);

            var Uid = int.Parse(UserID.Value);

            try
            {
                if (id == 0) return BadRequest();

                var result = await _service.DeleteTodoService(id, Uid);

                if (!result) return NotFound(new
                {
                    message = "Id not found"
                });

                return Ok(new
                {
                    message = "Todo Deleted Succesfully"
                });
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        [HttpPut("UpdateByID/{id:int}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateTodo(int id, [FromBody] TodoUpdateDTO dto)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized(new { message = "You must be logged in to perform this action." });
            }

                if (id == 0) return BadRequest();

            var UserID = User.Claims.FirstOrDefault(s=> s.Type == ClaimTypes.NameIdentifier);

            var Uid = int.Parse(UserID.Value);

            try
            {
                var result = await _service.UpdateTodoService(id, dto, Uid);

                if (!result) return NotFound();

                return Ok();
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Server Error - {ex.Message}");
            }
        }

        [HttpPatch("UpdateByIDPath/{id:int}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateTodoPatch(int id, [FromBody] UpdateTodoStatusIdDTO dto)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized(new { message = "You must be logged in to perform this action." });
            }

            if ( id == 0) return BadRequest();

            var UserID = User.Claims.FirstOrDefault(s=> s.Type == ClaimTypes.NameIdentifier);

            var UId = int.Parse(UserID.Value);

            try
            {
                var value = await _service.PatchTodoService(id, dto, UId);

                if (!value) return NotFound();

                return Ok();
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Server Error - {ex.Message}");
            }
        }


        [HttpGet("GetOneByID/{id?}")]
        [ProducesResponseType(typeof(GetOneDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<GetOneDTO>> GetOne(int id = 3)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized(new { message = "You must be logged in to perform this action." });
            }

            if (id == 0) return BadRequest();

            var UserID = User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier);

            var UId = int.Parse(UserID.Value);

            try
            {
                var result = await _service.GetOneService(id, UId);

                if(result == null) return NotFound();   

                return Ok(result);
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Server Error {ex.Message}");
            }
        }

        [HttpGet]
        [Route("Search")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<TodoGetAll>>> Search([FromQuery] int? status, [FromQuery] int? cat)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized(new { message = "You must be logged in to perform this action." });
            }

            var UserID = User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier);

            var UId = int.Parse(UserID.Value);

            var value = await _service.SearchService(status, cat, UId);

            if (value == null) return NotFound();

            return Ok(value);
        }
    }
}
