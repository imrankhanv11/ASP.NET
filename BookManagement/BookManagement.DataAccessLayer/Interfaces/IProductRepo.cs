using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookManagement.DataAccessLayer.Models;

namespace BookManagement.DataAccessLayer.Interfaces
{
    public interface IProductRepo : IGenericRepository<Book>
    {
        Task<Book> GetByBookName(string bookName);
    }
}
