using System;
using System.Collections.Generic;

namespace Todo.DataAccessLayer;

public partial class TodoModel
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int? CategoryId { get; set; }

    public int? StatusId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Category? Category { get; set; }

    public virtual Status? Status { get; set; }

    public virtual User User { get; set; } = null!;
}
