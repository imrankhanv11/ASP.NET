using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Service.DTO.Account.Response
{
    public class UserLoginResponstDTO
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
