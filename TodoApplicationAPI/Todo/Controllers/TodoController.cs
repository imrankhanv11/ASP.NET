using Azure.Core;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using Todo.ModelLayer.DTO.Request;
using Todo.ModelLayer.DTO.Response;
using Todo.ServiceLayer.Interface;

namespace Todo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _service;

        public TodoController(ITodoService service)
        {
            _service = service;
        }

        [HttpPost("AddTodo")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateTodo([FromBody] TodoAddDTO tododto)
        {
            if (tododto == null)
            {
                return BadRequest(new { Message = "Todo Not be empty"});
            }
            try
            {
                var result = await _service.TodoAddservice(tododto);

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
        public async Task<ActionResult<IEnumerable<TodoGetAll>>> GetAll()
        {
            try
            {
                var result = await _service.GetAllService();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error {ex.Message}");
            }
        }


        [HttpDelete("DeleteTodobyid/{id:int}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            try
            {
                if (id == 0) return BadRequest();

                var result = await _service.DeleteTodoService(id);

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
                //return StatusCode(500, $"Server Error {ex.Message}");
                throw;
            }
        }


        [HttpPut("UpdateByID/{id:int}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateTodo(int id, [FromBody] TodoUpdateDTO dto)
        {
            if(id == 0) return BadRequest();
            try
            {
                var result = await _service.UpdateTodoService(id, dto);

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
        public async Task<IActionResult> UpdateTodoPatch(int id, [FromBody] UpdateTodoStatusIdDTO dto)
        {
            if( id == 0) return BadRequest();

            try
            {
                var value = await _service.PatchTodoService(id, dto);

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
            if(id == 0) return BadRequest();
            
            try
            {
                var result = await _service.GetOneService(id);

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
        public async Task<ActionResult<IEnumerable<TodoGetAll>>> Search([FromQuery] int? status, [FromQuery] int? cat)
        {
            var value = await _service.SearchService(status, cat);

            if (value == null) return NotFound();

            return Ok(value);
        }
    }
}
