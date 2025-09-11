using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.DataAccessLayer.Data;
using Todo.DataAccessLayer.Interface;

namespace Todo.DataAccessLayer.Repository
{
    public class ValidationsTodo : IValidationsTodo
    {
        private readonly TodoContext _todoRepository;

        public ValidationsTodo(TodoContext context)
        {
            _todoRepository = context;
        }
        public async Task<bool> CategoryIDValidation(int? id)
        {
            if (id == null)
                return false;

            var category = await _todoRepository.Categories.FindAsync(id.Value);

            return category != null;
        }

        public async Task<bool> ShipperIDValidation(int? id)
        {
            var Status = await _todoRepository.Statuses.FindAsync(id);

            return Status != null;
        }

        public async Task<bool> TodoIdValidation(int id, int Uid)
        {
            var todo = await _todoRepository.Todos.Where(s=> s.Id == id && s.UserId == Uid).FirstOrDefaultAsync();

            return todo != null;
        }

        public async Task<bool> UserIDValidation(int id)
        {
            var User = await _todoRepository.Users.FindAsync(id);

            return User != null;
        }
    }
}
