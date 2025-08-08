using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Data.ViewModels
{
    public class CartItemViewModels
    {
        public int CartItemId { get; set; }

        public int? CartId { get; set; }

        public int? BookId { get; set; }

        public int? Quantity { get; set; }

        public decimal? Price { get; set; }
    }
}
