using EmployeeOnboarding.ServiceLayer.DTO.Department.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeOnboarding.ServiceLayer.Interfaces
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentResponseDTO>> GetAllDepartment();

        Task<IEnumerable<RoleResponseDTO>> GetAllRole(int deparmentId);

        Task<IEnumerable<LocationResponseDTO>> GetAllLocations();
    }
}
