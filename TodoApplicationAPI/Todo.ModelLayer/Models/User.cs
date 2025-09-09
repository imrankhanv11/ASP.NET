using System;
using System.Collections.Generic;

namespace Todo.DataAccessLayer;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<TodoModel> Todos { get; set; } = new List<TodoModel>();
}
