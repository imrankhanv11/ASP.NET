using LibararyManagement.Data.Data;
using LibararyManagement.Data.Interface;
using LibararyManagement.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibararyManagement.Data.Repository
{
    public class BookRepo : IBookRepo
    {
        private readonly LibararyContext _dbContext;

        public BookRepo(LibararyContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Book> AddBookRepo(Book book)
        {
            await _dbContext.Books.AddAsync(book);
            await _dbContext.SaveChangesAsync();

            return book;
        }

        public async Task<IEnumerable<Book>> GetAllRepo()
        {
            var result = await _dbContext.Books.Include(s=> s.Category).ToListAsync();

            return result;
        }

        public async Task<IEnumerable<Category>> GetAllCatRepo()
        {
            var result = await _dbContext.Categories.ToListAsync();

            return result;
        }

        public async Task DeleteBookRepo(int id)
        {
            var book = await _dbContext.Books.FindAsync(id);

            _dbContext.Books.Remove(book);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<Book> GetoneBookByIdRepo(int id)
        {

            var book = await _dbContext.Books.FindAsync(id);

            return book;
        }

        public async Task<Book> UpdateBookRepo(Book book)
        {
            _dbContext.Update(book);

            await _dbContext.SaveChangesAsync();

            return book;
        }

        public async Task<Book> GetBookByName(string name)
        {
            var book = await _dbContext.Books.FirstOrDefaultAsync(s=> s.BookName == name);

            return book;
        }
    }
}
