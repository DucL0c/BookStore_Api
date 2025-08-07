using System;
using System.Collections.Generic;

namespace ShopBook.Data.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string? Name { get; set; }

    public bool? IsLeaf { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
