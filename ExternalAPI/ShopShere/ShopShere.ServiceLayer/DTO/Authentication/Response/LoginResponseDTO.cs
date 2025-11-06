using ShopSphere.DataAccessLayer.Models;
using ShopSphere.ServiceLayer.DTO.Authentication.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ServiceLayer.DTO.Authentications.Response
{
    public class LoginResponseDTO
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public LoginUserResponseDTO User { get; set; }
    }
}
