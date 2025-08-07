using System;
using System.Collections.Generic;

namespace ShopBook.Data.Models;

public partial class Author
{
    public int AuthorId { get; set; }

    public string? Name { get; set; }

    public string? Slug { get; set; }

    public virtual ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
}
