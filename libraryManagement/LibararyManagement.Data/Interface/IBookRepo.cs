using LibararyManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibararyManagement.Data.Interface
{
    public interface IBookRepo
    {
        Task<Book> AddBookRepo(Book book);

        Task<IEnumerable<Book>> GetAllRepo();

        Task<IEnumerable<Category>> GetAllCatRepo();

        Task DeleteBookRepo(int id);


        Task<Book> GetoneBookByIdRepo(int id);

        Task<Book> UpdateBookRepo(Book book);

        Task<Book> GetBookByName(string name);
    }
}
