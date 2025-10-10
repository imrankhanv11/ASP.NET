using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.ServiceLayer.DTO.Authentications.Request;

namespace ECommerce.ServiceLayer.FluentValidations
{
    public class RegisterUserDTOValidations : AbstractValidator<RegisterUserDTO>
    {
        public RegisterUserDTOValidations() 
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("User Name Can't be Empty")
                .MinimumLength(3).WithMessage("User Name atlest need 3 char")
                .MaximumLength(100).WithMessage("Not more than 100");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email can't be empty")
                .EmailAddress().WithMessage("Email not in good");

            RuleFor(x => x.PasswordHash)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
                .Matches("[0-9]").WithMessage("Password must contain at least one digit")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character (!@#$%^&* etc.)");

            RuleFor(x => x.ConfirmPasswordHash)
                .Equal(x => x.PasswordHash).WithMessage("Confirm Password must match Password");

        }
    }
}
