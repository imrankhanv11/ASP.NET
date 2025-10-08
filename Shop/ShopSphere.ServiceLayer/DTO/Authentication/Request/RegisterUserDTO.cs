using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSphere.ServiceLayer.DTO.Authentication.Request
{
    public class RegisterUserDTO
    {
        [Required(ErrorMessage ="Name Filed is Required")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Email id is Required for Registrations")]
        [EmailAddress(ErrorMessage = "Email not in Format")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required for Registrations")]
        [MinLength(6, ErrorMessage = "Password must be At least 6 characters values")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
        ErrorMessage = "Password must contain Uppercase, Lowercase, Number, and Special character")]
        public string PasswordHash { get; set; } = null!;
    }
}
