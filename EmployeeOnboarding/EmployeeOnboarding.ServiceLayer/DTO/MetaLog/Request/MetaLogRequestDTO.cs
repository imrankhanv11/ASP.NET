using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeOnboarding.ServiceLayer.DTO.MetaLog.Request
{
    public class MetaLogRequestDTO
    {
        [Required(ErrorMessage = "EmployeeId is Required")]
        public int EmployeeId { get; set; }


        [Required(ErrorMessage = "DepartmentId is Required")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "RoleId is Required")]
        public int RoleId { get; set; }


        [Required(ErrorMessage = "JoiningDate is Required")]
        public DateOnly JoiningDate { get; set; }
    }
}
