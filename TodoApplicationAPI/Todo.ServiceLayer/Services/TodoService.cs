using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.DataAccessLayer;
using Todo.DataAccessLayer.Data;
using Todo.DataAccessLayer.Interface;
using Todo.DataAccessLayer.Repository;
using Todo.ModelLayer;
using Todo.ModelLayer.DTO.Request;
using Todo.ModelLayer.DTO.Response;
using Todo.ModelLayer.ValidationResponse;
using Todo.ServiceLayer.Interface;

namespace Todo.ServiceLayer.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepositoy _respository;
        private readonly IValidationsTodo _validation;

        public TodoService(ITodoRepositoy repositoy, IValidationsTodo validations)
        {
            _respository = repositoy;
            _validation = validations;
        }


        public async Task<ResultSet<TodoAddResponseDTO>> TodoAddservice(TodoAddDTO dto)
        {

            var outputReturn = new ResultSet<TodoAddResponseDTO>();

            if (!await _validation.UserIDValidation(dto.UserId))
            {
                outputReturn.ErrorMessage = "User ID not found";
                outputReturn.Field = "UserID";
                return outputReturn;
            }

            if(!await _validation.CategoryIDValidation(dto.CategoryId))
            {
                outputReturn.ErrorMessage = "Cat ID not found";
                outputReturn.Field = "Cat ID";
                return outputReturn;
            }

            if(!await _validation.ShipperIDValidation(dto.StatusId)) 
            {
                outputReturn.ErrorMessage = "Status ID not found";
                outputReturn.Field = "status";
                return outputReturn;
            }

            var newTodo = new TodoModel
            {
                UserId = dto.UserId,
                StatusId = dto.StatusId,
                CategoryId = dto.CategoryId,
                Title = dto.Title,
                Description = dto.Description,
            };

            var result = await _respository.TodoAddRepo(newTodo);

            var value = new TodoAddResponseDTO
            {
                Id = result.Id,
                UserId = result.UserId,
                StatusId = result.StatusId,
                CategoryId = result.CategoryId,
                Title = result.Title,
                Description= result.Description
            };

            outputReturn.Data = value;
            outputReturn.Success = true;

            return outputReturn;
            
        }

        public async Task<IEnumerable<TodoGetAll>> GetAllService()
        {
            var todo = await _respository.TodoGetAllRepo();

            var todoresult = todo.Select(s => new TodoGetAll
            {
                Name = s.User.Name,
                Category = s.Category.Name,
                Status = s.Status.Name,
                Title = s.Title,
                Description = s.Description,
                CreatedDate = DateOnly.FromDateTime((DateTime)s.CreatedAt)
            });
        
            return todoresult;
        }

        public async Task<bool> DeleteTodoService(int id)
        {
            if(!await _validation.TodoIdValidation(id))
            {
                throw new KeyNotFoundException("hii");
            }
            return await _respository.DeleteTodoRepo(id);
        }

        public async Task<bool> UpdateTodoService(int id, TodoUpdateDTO dto)
        {
            if (!await _validation.TodoIdValidation(id))
            {
                return false;
            }
            return await _respository.UpdateTodoRepo(id, dto);
        }

        public async Task<bool> PatchTodoService(int id, UpdateTodoStatusIdDTO dto)
        {
            if (!await _validation.TodoIdValidation(id))
            {
                return false;
            }
            return await _respository.PatchTodoRepo(id, dto);
        }

        public async Task<GetOneDTO> GetOneService(int id)
        {
            var value = await _respository.GetOneRepo(id);

            return value;
        }

        public async Task<IEnumerable<TodoGetAll>> SearchService(int? status, int? cat)
        {
            var todo = await _respository.SearchRepo(status, cat);

            var todoresult = todo.Select(s => new TodoGetAll
            {
                Name = s.User.Name,
                Category = s.Category.Name,
                Status = s.Status.Name,
                Title = s.Title,
                Description = s.Description,
                CreatedDate = DateOnly.FromDateTime((DateTime)s.CreatedAt)
            });

            return todoresult;

        }
    }
}