using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeOnboarding.ServiceLayer.DTO.Department.Response
{
    public class LocationResponseDTO
    {
        public int Id { get; set; }

        public string LocationName { get; set; } = null!;
    }
}
