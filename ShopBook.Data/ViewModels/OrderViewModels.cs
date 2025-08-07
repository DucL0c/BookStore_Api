using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Data.ViewModels
{
    public class OrderViewModels
    {
        public int OrderId { get; set; }

        public int? UserId { get; set; }

        public DateTime? OrderDate { get; set; }

        public decimal? TotalAmount { get; set; }

        public string? Status { get; set; }

        public string? ShippingAddress { get; set; }

        public string? PaymentMethod { get; set; }
    }
}
