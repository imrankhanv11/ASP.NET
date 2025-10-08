using System;
using System.Collections.Generic;

namespace ShopSphere.DataAccessLayer.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public int RoleId { get; set; }

    public DateOnly CreatedAt { get; set; }

    public virtual Cart? Cart { get; set; }

    public virtual Role Role { get; set; } = null!;
}
