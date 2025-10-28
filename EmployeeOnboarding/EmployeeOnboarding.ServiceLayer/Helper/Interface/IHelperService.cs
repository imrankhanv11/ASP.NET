using EmployeeOnboarding.ServiceLayer.DTO.Helper.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeOnboarding.ServiceLayer.Helper.Interface
{
    public interface IHelperService
    {
        Task NotifyHodHelperService(int departmentId, NotifyHodRequestDTO requestDto);
    }
}
