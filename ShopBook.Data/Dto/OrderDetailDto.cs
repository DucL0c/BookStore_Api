using ShopBook.Model.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Data.Dto
{
    public class OrderDetailDto
    {
        public int OrderId { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? Status { get; set; }
        public string? ShippingAddress { get; set; }
        public string? PaymentMethod { get; set; }

        public UsersDto? User { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
    }

    public class OrderItemDto
    {
        public int OrderItemId { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public BooksDto? Book { get; set; }
    }

    public class UsersDto
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }

    public class BooksDto
    {
        public int BookId { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }

        public virtual ICollection<BookImageOrderDto> BookImages { get; set; } = new List<BookImageOrderDto>();

    }

    public class BookImageOrderDto
    {
        public int ImageId { get; set; }

        public string? BaseUrl { get; set; }

        public string? SmallUrl { get; set; }

        public string? MediumUrl { get; set; }

        public string? LargeUrl { get; set; }

        public string? ThumbnailUrl { get; set; }

        public bool? IsGallery { get; set; }
    }
}
