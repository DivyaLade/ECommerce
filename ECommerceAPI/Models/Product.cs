using System;
using System.Collections.Generic;

namespace ECommerceAPI.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public int? Stock { get; set; }

    public string? Category { get; set; }

    //public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
