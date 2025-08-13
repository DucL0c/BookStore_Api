using System;
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
    }
}
