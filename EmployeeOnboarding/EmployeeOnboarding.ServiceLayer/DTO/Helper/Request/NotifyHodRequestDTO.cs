using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeOnboarding.ServiceLayer.DTO.Helper.Request
{
    public class NotifyHodRequestDTO
    {

        public int Id { get; set; }

        public string FullName { get; set; } = null!;

        public int Roleid { get; set; }

        public DateOnly JoiningDate { get; set; }

    }
}
