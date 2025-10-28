using EmployeeOnboarding.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeOnboarding.DataAccessLayer.Interfaces
{
    public interface IDepartmentRepo
    {
        Task<IEnumerable<Department>> GetDepartmentsRepo();

        Task<IEnumerable<Role>> GetAllRolesRepo(int DepartmentId);

        Task<bool> CheckDepartmentExits(int deparmentid);

        Task<IEnumerable<Location>> GetAllLocations();
    }
}
