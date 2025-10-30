using BookManagement.DataAccessLayer.Data;
using BookManagement.DataAccessLayer.Interfaces;
using BookManagement.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.DataAccessLayer.Repositories
{
    public class ProductRepo : GenericRepository<Book>, IProductRepo
    {
        private readonly BookContext _dbContext;
        public ProductRepo(BookContext context) : base(context)
        {
            _dbContext = context;
 
        }

        public async Task<Book> GetByBookName(string bookName)
        {
            var output = await _dbContext.Books.Where(s => s.Title == bookName ).Include(m => m.Category).FirstOrDefaultAsync();

            return output;
        }
    }
}
