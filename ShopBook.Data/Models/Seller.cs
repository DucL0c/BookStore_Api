using System;
using System.Collections.Generic;

namespace ShopBook.Data.Models;

public partial class Seller
{
    public int SellerId { get; set; }

    public string? Name { get; set; }

    public string? Link { get; set; }

    public string? Logo { get; set; }

    public virtual ICollection<BookSeller> BookSellers { get; set; } = new List<BookSeller>();
}
