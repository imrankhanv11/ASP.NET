using BookManagement.DataAccessLayer.Data;
using BookManagement.DataAccessLayer.Interfaces;
using BookManagement.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.DataAccessLayer.Repositories
{
    public class CatRepo : GenericRepository<Category>, ICatRepo
    {
        private readonly BookContext bookContext;
        public CatRepo(BookContext context) : base(context)
        {
            bookContext = context;
        }

        public async Task<IEnumerable<Category>> catRepo(int id)
        {
            var value = await bookContext.Categories.Include(s=> s.Books).Where(m=> m.CategoryId == id).ToListAsync();

            return value;
        }
    }
}
