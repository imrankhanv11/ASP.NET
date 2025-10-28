using System;
using System.Collections.Generic;

namespace EmployeeOnboarding.DataAccessLayer.Models;

public partial class Employee
{
    public int Id { get; set; }

    public int SubmissionId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public int DepartmentId { get; set; }

    public int RoleId { get; set; }

    public int LocationId { get; set; }

    public int Experience { get; set; }

    public DateOnly JoiningDate { get; set; }

    public int Ctc { get; set; }

    public string? Status { get; set; }

    public DateOnly ProbationEndDate { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual Location Location { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
