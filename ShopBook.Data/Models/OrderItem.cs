using System;
using System.Collections.Generic;

namespace ShopBook.Data.Models;

public partial class OrderItem
{
    public int OrderItemId { get; set; }

    public int? OrderId { get; set; }

    public int? BookId { get; set; }

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }

    public virtual Book? Book { get; set; }

    public virtual Order? Order { get; set; }
}
