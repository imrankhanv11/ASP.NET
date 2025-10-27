using LibararyManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibararyManagement.Data.Interface
{
    public interface IBorrowRepo
    {
        Task<Borrow> BorrowBookRepo(Borrow book);

        Task<Borrow> BookBorrowedExits(int bookId, int userId);

        Task<Borrow> BorrowGetById(int id);

        Task<Borrow> BookNotDelted(int bookid);
    }
}
