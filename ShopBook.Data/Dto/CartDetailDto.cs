using ShopBook.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Data.Dto
{
    public class CartDetailDto
    {
        public int CartId { get; set; }

        public int? UserId { get; set; }

        public DateTime? CreatedAt { get; set; }

        public virtual ICollection<CartItemsDto> CartItems { get; set; } = new List<CartItemsDto>();

        public virtual UserCartDto? User { get; set; }
    }

    public class CartItemsDto
    {
        public int CartItemId { get; set; }

        public int? BookId { get; set; }

        public int? Quantity { get; set; }

        public decimal? Price { get; set; }
        public BookCartDto? Book { get; set; }
    }

    public class UserCartDto
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }

    public class BookCartDto
    {
        public int BookId { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public virtual ICollection<BookSellerCartDto> BookSellers { get; set; } = new List<BookSellerCartDto>();

        public virtual ICollection<BookImageCartDto> BookImages { get; set; } = new List<BookImageCartDto>();
    }

    public class BookSellerCartDto
    {
        public int Id { get; set; }

        public string? Sku { get; set; }

        public decimal? Price { get; set; }

        public string? ProductId { get; set; }

        public int? StoreId { get; set; }

        public bool? IsBestStore { get; set; }
        public SellerCartDto? Seller { get; set; }
    }

    public class SellerCartDto
    {
        public int SellerId { get; set; }

        public string? Name { get; set; }

        public string? Link { get; set; }

        public string? Logo { get; set; }

    }

    public class BookImageCartDto
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
