using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.DataAccessLayer.Data;
using Todo.DataAccessLayer.Interface;
using Todo.ModelLayer.DTO.Response;

namespace Todo.DataAccessLayer.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly TodoContext _context;

        public CategoryRepository(TodoContext context)
        {
            _context = context;
        }

        public async Task<AddCatResponceDTO> AddCatRepo(Category cat)
        {
            var result = await _context.Categories.AddAsync(cat);

            await _context.SaveChangesAsync();

            return new AddCatResponceDTO
            {
                id = cat.Id,
            };
        }
    }
}
