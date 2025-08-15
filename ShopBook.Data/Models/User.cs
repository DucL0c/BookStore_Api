using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopBook.Data.Models;

public partial class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserId { get; set; }


    public string? Name { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
    public string? RefreshToken { get; set; }
    public DateTime? ExpiryDate { get; set; }

    public string? Role { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? Gender { get; set; }

    public DateOnly? BirthDay { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<ProductReview> ProductReviews { get; set; } = new List<ProductReview>();
}
