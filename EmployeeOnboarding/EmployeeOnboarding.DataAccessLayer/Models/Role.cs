using System;
using System.Collections.Generic;

namespace EmployeeOnboarding.DataAccessLayer.Models;

public partial class Role
{
    public int Id { get; set; }

    public int DepartmentId { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<MetaLog> MetaLogs { get; set; } = new List<MetaLog>();
}
