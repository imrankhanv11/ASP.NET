using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.DataAccessLayer;
using Todo.DataAccessLayer.Data;
using Todo.DataAccessLayer.Interface;
using Todo.DataAccessLayer.Repository;
using Todo.ModelLayer.DTO.Request;
using Todo.ModelLayer.DTO.Response;
using Todo.ServiceLayer.Interface;
using Todo.ModelLayer;

namespace Todo.ServiceLayer.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepositoy _respository;

        public TodoService(ITodoRepositoy repositoy)
        {
            _respository = repositoy;
        }
        public async Task<TodoAddResponseDTO> TodoAddservice(TodoAddDTO dto)
        {
            var newTodo = new TodoModel
            {
                UserId = dto.UserId,
                StatusId = dto.StatusId,
                CategoryId = dto.CategoryId,
                Title = dto.Title,
                Description = dto.Description,
            };

            var result = await _respository.TodoAddRepo(newTodo);

            return new TodoAddResponseDTO
            {
                Id = result.Id,
                UserId = result.UserId,
                StatusId = result.StatusId,
                CategoryId = result.CategoryId,
                Title = result.Title,
                Description= result.Description
            };
        }

        public async Task<IEnumerable<TodoGetAll>> GetAllService()
        {
            var todo = await _respository.TodoGetAllRepo();
            //return await _respository.TodoGetAllRepo();
            return todo;
        }

        public async Task<bool> DeleteTodoService(int id)
        {
            return await _respository.DeleteTodoRepo(id);
        }

        public async Task<bool> UpdateTodoService(int id, TodoUpdateDTO dto)
        {
            return await _respository.UpdateTodoRepo(id, dto);
        }

        public async Task<bool> PatchTodoService(int id, UpdateTodoStatusIdDTO dto)
        {
            return await _respository.PatchTodoRepo(id, dto);
        }

        public async Task<GetOneDTO> GetOneService(int id)
        {
            var value = await _respository.GetOneRepo(id);

            return value;
        }
    }
}