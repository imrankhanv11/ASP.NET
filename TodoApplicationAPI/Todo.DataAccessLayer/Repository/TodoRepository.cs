using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.DataAccessLayer.Data;
using Todo.DataAccessLayer.Interface;
using Todo.ModelLayer.DTO.Request;
using Todo.ModelLayer.DTO.Response;

namespace Todo.DataAccessLayer.Repository
{
    public class TodoRepository : ITodoRepositoy
    {
        private readonly TodoContext _dbContext;

        public TodoRepository(TodoContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<TodoModel> TodoAddRepo(TodoModel todo)
        {
            _dbContext.Todos.Add(todo);

            await _dbContext.SaveChangesAsync();

            return todo;
        }

        public async Task<IEnumerable<TodoGetAll>> TodoGetAllRepo()
        {
            var value = await _dbContext.Todos.Select(s => new TodoGetAll
            {
                Name = s.User.Name,
                Category = s.Category.Name,
                Title = s.Title,
                Description = s.Description,
                Status = s.Status.Name,
                CreatedDate = DateOnly.FromDateTime((DateTime)s.CreatedAt)
            }).ToListAsync();

            return value;
        }

        public async Task<bool> DeleteTodoRepo(int id)
        {
            var value = _dbContext.Todos.Find(id);

            if(value == null)
            {
                return false;
            }

            _dbContext.Todos.Remove(value);

            await _dbContext.SaveChangesAsync();

            return true;

        }

        public async Task<bool> UpdateTodoRepo(int id, TodoUpdateDTO model)
        {
            var value = await _dbContext.Todos.FindAsync(id);

            if(value == null) return false;

            value.UserId = model.UserId;
            value.StatusId = model.StatusId;
            value.CategoryId = model.CategoryId;
            value.Title = model.Title;
            value.Description = model.Description;
            

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> PatchTodoRepo(int id, UpdateTodoStatusIdDTO dto)
        {
            var value = await _dbContext.Todos.FindAsync(id);

            if(value == null) return false;

            value.StatusId = dto.StatusID;

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<GetOneDTO> GetOneRepo(int id)
        {
            var value = await _dbContext.Todos
                .Where(s => s.Id == id)
                .Select(s => new GetOneDTO
                {
                    Name = s.User.Name,
                    Category = s.Category.Name,
                    Title = s.Title,
                    Description = s.Description,
                    Status = s.Status.Name,
                    CreatedDate = DateOnly.FromDateTime((DateTime)s.CreatedAt)
                }).FirstOrDefaultAsync();

            return value;
        }
    }
}
