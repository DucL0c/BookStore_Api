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
    }
}
