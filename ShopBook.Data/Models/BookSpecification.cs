using System;
using System.Collections.Generic;

namespace ShopBook.Data.Models;

public partial class BookSpecification
{
    public int Id { get; set; }
    public int BookId { get; set; }

    public string SpecName { get; set; } = null!;

    public string? SpecValue { get; set; }

    public string SpecCode { get; set; } = null!;

    public virtual Book Book { get; set; } = null!;
}
