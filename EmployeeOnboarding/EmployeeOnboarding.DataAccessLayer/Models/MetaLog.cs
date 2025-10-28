using System;
using System.Collections.Generic;

namespace EmployeeOnboarding.DataAccessLayer.Models;

public partial class MetaLog
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public int DepartmentId { get; set; }

    public int RoleId { get; set; }

    public DateOnly JoiningDate { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
