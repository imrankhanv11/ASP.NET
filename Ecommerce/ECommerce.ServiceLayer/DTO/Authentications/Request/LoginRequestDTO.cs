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
        [Required(ErrorMessage = "Required")]
        [EmailAddress(ErrorMessage = "not match the Email Format")]
        public string Email { get; set; }

        // Checking Password
        public string Password { get; set; }
    }
}
