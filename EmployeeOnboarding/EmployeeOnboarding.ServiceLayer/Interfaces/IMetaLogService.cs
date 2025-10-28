using EmployeeOnboarding.ServiceLayer.DTO.MetaLog.Request;
using EmployeeOnboarding.ServiceLayer.DTO.MetaLog.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeOnboarding.ServiceLayer.Interfaces
{
    public interface IMetaLogService
    {
        Task<MetaLogResponseDTO> MetaLogServiceCall(MetaLogRequestDTO dto);
    }
}
