using BookManagement.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.DataAccessLayer.Interfaces
{
    public interface ICatRepo : IGenericRepository<Category>
    {
        Task<IEnumerable<Category>> catRepo(int id);
    }
}
