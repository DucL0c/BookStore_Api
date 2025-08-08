using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopBook.Data.Models;

public partial class Book
{
    public int BookId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? ShortDescription { get; set; }

    public double? RatingAverage { get; set; }

    public decimal? OriginalPrice { get; set; }

    public decimal? ListPrice { get; set; }

    public int? QuantitySold { get; set; }

    [NotMapped]
    public string? QuantitySoldText { get; set; } // Không lưu DB

    public int? CategoryId { get; set; }

    public virtual ICollection<BookImage> BookImages { get; set; } = new List<BookImage>();

    public virtual ICollection<BookSeller> BookSellers { get; set; } = new List<BookSeller>();

    public virtual ICollection<BookSpecification> BookSpecifications { get; set; } = new List<BookSpecification>();

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Category? Category { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<ProductReview> ProductReviews { get; set; } = new List<ProductReview>();

    public virtual ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
}
