using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.ModelLayer.DTO.Response
{
    public class LoginResponseDTO
    {
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;

        public string RefreshToken {  get; set; } = string.Empty ;
        public DateTime Expiration { get; set; }

    }
}
