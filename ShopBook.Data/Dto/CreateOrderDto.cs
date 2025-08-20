using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Data.Dto
{
    public class CreateOrderDto
    {
        public int UserId { get; set; }
        public string? ShippingAddress { get; set; }
        public string? PaymentMethod { get; set; }

        public virtual ICollection<BookOrderDto> Items { get; set; } = new List<BookOrderDto>();
        public string? Typess { get; set; }
    }

    public class BookOrderDto
    {
        public int BookId { get; set; }
        public int Quantity { get; set; }
    }
}
