using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ServiceLayer.DTO.Authentications.Request
{
    public class LoginRequestDTO
    {
        [Required(ErrorMessage = "Email ID is Required")]
        [EmailAddress(ErrorMessage = "Email not Match the Format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password Requried for Login")]
        public string Password { get; set; }
    }
}
