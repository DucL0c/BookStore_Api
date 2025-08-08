using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ShopBook.Data.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string? Name { get; set; }

    public bool? IsLeaf { get; set; }

    [JsonIgnore] // Ngăn EF trả về danh sách sách bên trong category
    public ICollection<Book> Books { get; set; }
}
