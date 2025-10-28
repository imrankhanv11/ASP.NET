using System;
using System.Collections.Generic;

namespace EmployeeOnboarding.DataAccessLayer.Models;

public partial class Location
{
    public int Id { get; set; }

    public string LocationName { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Hod> Hods { get; set; } = new List<Hod>();
}
