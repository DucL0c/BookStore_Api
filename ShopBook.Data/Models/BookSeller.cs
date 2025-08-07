using System;
using System.Collections.Generic;

namespace ShopBook.Data.Models;

public partial class BookSeller
{
    public int Id { get; set; }
    public int BookId { get; set; }

    public int SellerId { get; set; }

    public string? Sku { get; set; }

    public decimal? Price { get; set; }

    public string? ProductId { get; set; }

    public int? StoreId { get; set; }

    public bool? IsBestStore { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Seller Seller { get; set; } = null!;
}
