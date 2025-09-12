using System;
using System.Collections.Generic;

namespace BookManagement.DataAccessLayer.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int UserId { get; set; }

    public int BookId { get; set; }

    public int Quantity { get; set; }

    public DateTime OrderDate { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
