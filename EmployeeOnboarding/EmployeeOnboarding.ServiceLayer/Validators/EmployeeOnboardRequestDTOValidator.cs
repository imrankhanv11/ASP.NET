using FluentValidation;
using EmployeeOnboarding.ServiceLayer.DTO.Employee.Request;

namespace EmployeeOnboarding.ServiceLayer.Validators
{
    public class EmployeeOnboardRequestDTOValidator : AbstractValidator<EmployeeOnboardRequestDTO>
    {
        public EmployeeOnboardRequestDTOValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("FirstName is required")
                .MinimumLength(3).WithMessage("FirstName must be at least 3 characters long");

            RuleFor(x => x.MiddleName)
                .MaximumLength(50).WithMessage("MiddleName cannot exceed 50 characters")
                .When(x => !string.IsNullOrEmpty(x.MiddleName));

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("LastName is required")
                .MinimumLength(3).WithMessage("LastName must be at least 3 characters long");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("PhoneNumber is required")
                .Matches(@"^[0-9]{10}$").WithMessage("PhoneNumber must be 10 digits");

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage("DepartmentId is required and must be greater than 0");

            RuleFor(x => x.RoleId)
                .GreaterThan(0).WithMessage("RoleId is required and must be greater than 0");

            RuleFor(x => x.LocationId)
                .GreaterThan(0).WithMessage("LocationId is required and must be greater than 0");

            RuleFor(x => x.Experience)
                .GreaterThanOrEqualTo(0).WithMessage("Experience must be a positive number");

            RuleFor(x => x.JoiningDate)
                .NotEmpty().WithMessage("JoiningDate is required")
                .Must(date => date <= DateOnly.FromDateTime(DateTime.Now))
                .WithMessage("JoiningDate cannot be in the future");

            RuleFor(x => x.Ctc)
                .GreaterThan(0).WithMessage("Ctc must be greater than 0");
        }
    }
}
