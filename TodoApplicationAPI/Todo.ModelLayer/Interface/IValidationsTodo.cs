using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.DataAccessLayer.Interface
{
    public interface IValidationsTodo
    {
        Task<bool> UserIDValidation(int id);
        Task<bool> CategoryIDValidation(int? id);
        Task<bool> ShipperIDValidation(int? id);
        Task<bool> TodoIdValidation(int id, int Uid);
    }
}
