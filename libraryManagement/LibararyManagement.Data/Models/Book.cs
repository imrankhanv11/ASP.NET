using System;
using System.Collections.Generic;

namespace LibararyManagement.Data.Models;

public partial class Book
{
    public int Id { get; set; }

    public string BookName { get; set; } = null!;

    public string Author { get; set; } = null!;

    public int Price { get; set; }

    public int Stock { get; set; }

    public int CategoryId { get; set; }

    public string? PictureLink { get; set; }

    public virtual ICollection<Borrow> Borrows { get; set; } = new List<Borrow>();

    public virtual Category Category { get; set; } = null!;
}
