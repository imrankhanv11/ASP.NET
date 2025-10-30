using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.ServiceLayer.DTO.Login.Response
{
    public class LoginResponseUserDTO
    {
        public int UserId { get; set; }

        public string Username { get; set; } = null!;

        public string Role { get; set; } = null!;
    }
}
