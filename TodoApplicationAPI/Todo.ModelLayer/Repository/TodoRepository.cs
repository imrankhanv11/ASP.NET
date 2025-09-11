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

        public async Task<IEnumerable<TodoModel>> TodoGetAllRepo(int userID)
        {
            var value = await _dbContext.Todos
                .Include(s => s.Category)
                .Include(s => s.User)
                .Include(m => m.Status)
                .Where(s=> s.UserId  == userID)
                .ToListAsync();

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

        public async Task<bool> UpdateTodoRepo(int id, TodoUpdateDTO model, int Uid)
        {
            var value = await _dbContext.Todos.FindAsync(id);

            if(value == null) return false;

            value.UserId = Uid;
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

        public async Task<GetOneDTO> GetOneRepo(int id, int UId)
        {
            var value = await _dbContext.Todos
                .Where(s => s.Id == id)
                .Where(s=> s.UserId == UId)
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

        public async Task<IEnumerable<TodoModel>> SearchRepo(int? status, int? cat, int UId)
        {
            var value = _dbContext.Todos.AsQueryable();

            value = value
                .Where(s=> s.UserId == UId)
                .Where(s => !status.HasValue || s.StatusId == status)
                .Where(s => !cat.HasValue || s.CategoryId == cat)
                .Include(s => s.Status)
                .Include(cat => cat.Category)
                .Include(s=> s.User);

            var task = await value.ToListAsync();

            return task;
        }
    }
}
