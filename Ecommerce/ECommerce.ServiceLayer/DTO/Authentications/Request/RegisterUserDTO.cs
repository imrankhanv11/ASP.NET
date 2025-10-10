using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ServiceLayer.DTO.Authentications.Request
{
    public class RegisterUserDTO
    {
        [Required(ErrorMessage = "UserName Required")]
        [MinLength(3, ErrorMessage = "MinLength 3 Char")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage ="Required")]
        [EmailAddress(ErrorMessage = "not match the Email Format")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
        ErrorMessage = "Password must contain uppercase, lowercase, number, and special character")]
        public string PasswordHash { get; set; } = null!;

        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("PasswordHash", ErrorMessage = "Passwords do not match")]
        public string ConfirmPasswordHash { get; set; } = null!;
    }
}
