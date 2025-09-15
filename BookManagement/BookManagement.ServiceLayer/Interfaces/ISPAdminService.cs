using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.ServiceLayer.Interfaces
{
    public interface ISPAdminService
    {
        Task<bool> AddAdminService(int id);
    }
}
