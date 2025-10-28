using EmployeeOnboarding.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeOnboarding.DataAccessLayer.Interfaces
{
    public interface IHodRepositroy
    {
        Task<IEnumerable<Hod>> GetDeparmentHodNotify(int departmentId);
    }
}
