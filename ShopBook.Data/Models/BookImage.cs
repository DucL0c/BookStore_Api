using System;
using System.Collections.Generic;

namespace ShopBook.Data.Models;

public partial class BookImage
{
    public int ImageId { get; set; }

    public int? BookId { get; set; }

    public string? BaseUrl { get; set; }

    public string? SmallUrl { get; set; }

    public string? MediumUrl { get; set; }

    public string? LargeUrl { get; set; }

    public string? ThumbnailUrl { get; set; }

    public bool? IsGallery { get; set; }

    public virtual Book? Book { get; set; }
}
