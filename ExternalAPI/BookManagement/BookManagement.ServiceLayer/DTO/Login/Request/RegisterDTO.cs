using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.ServiceLayer.DTO.Login.Request
{
    public class RegisterDTO
    {
        [Required]
        [MinLength(5, ErrorMessage ="UserName Need minimum 5 Char")]
        public string UserName { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password Need minimum 8 Char")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Confirm Password and password must be same")]
        public string ConfirmPassword { get; set; }
    }
}
