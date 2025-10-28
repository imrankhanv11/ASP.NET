using System;
using System.Collections.Generic;

namespace EmployeeOnboarding.DataAccessLayer.Models;

public partial class Department
{
    public int Id { get; set; }

    public string DepartmentName { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Hod> Hods { get; set; } = new List<Hod>();

    public virtual ICollection<MetaLog> MetaLogs { get; set; } = new List<MetaLog>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
