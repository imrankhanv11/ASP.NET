using EmployeeOnboarding.ServiceLayer.DTO.Employee.Request;
using EmployeeOnboarding.ServiceLayer.DTO.Employee.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeOnboarding.ServiceLayer.Interfaces
{
    public interface IEmployeeService
    {
        Task<EmployeeOnboardingResponseDTO> EmployeeOnboardingService(EmployeeOnboardRequestDTO empDto);
    }
}
