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
    public class BorrowRepo : IBorrowRepo
    {
        private readonly LibararyContext _dbContext;

        public BorrowRepo(LibararyContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Borrow> BorrowBookRepo(Borrow book)
        {
            _dbContext.Borrows.Add(book);

            await _dbContext.SaveChangesAsync();

            return book;
        }

        public async Task<Borrow> BookBorrowedExits(int bookId, int userId)
        {
            var book = await _dbContext.Borrows.FirstOrDefaultAsync(s=> s.BookId ==bookId && s.UserId == userId && s.Status == "Pending" || s.Status == "Overdue");

            return book;
        }

        public  async Task<Borrow> BorrowGetById(int id)
        {

            var borrowBook = await _dbContext.Borrows.Include(s => s.Book).FirstOrDefaultAsync(s => s.Id == id);

            return borrowBook;
        }

        public async Task<Borrow> BookNotDelted(int bookid)
        {
            var book = await _dbContext.Borrows.FirstOrDefaultAsync(s=> s.BookId == bookid && s.Status =="Pending" || s.Status == "Overdue");

            return book;
        }

    }
}
