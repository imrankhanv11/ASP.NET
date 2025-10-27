using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Service.DTO.Account.Response
{
    public class UserRegisterResponseDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Role { get; set; }

        public string Email { get; set; } = null!;
    }
}
