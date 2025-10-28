using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeOnboarding.ServiceLayer.DTO.Employee.Request
{
    public class EmployeeOnboardRequestDTO
    {
        public string FirstName { get; set; } = null!;

        public string? MiddleName { get; set; }

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public int DepartmentId { get; set; }

        public int RoleId { get; set; }

        public int LocationId { get; set; }

        public int Experience { get; set; }

        public DateOnly JoiningDate { get; set; }

        public int Ctc { get; set; }
    }
}
