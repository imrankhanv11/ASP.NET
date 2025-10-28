using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeOnboarding.ServiceLayer.DTO.MetaLog.Response
{
    public class MetaLogResponseDTO
    {
        public int Id { get; set; }
        public string Status {  get; set; }
        public string Message { get; set; }
        public string ProcessedBy { get; set; }
        public DateTime ProcessedDate { get; set; }
    }
}
