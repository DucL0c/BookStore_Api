using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Data.ViewModels
{
    public class BookSellerViewModels
    {
        public int Id { get; set; }
        public int BookId { get; set; }

        public int SellerId { get; set; }

        public string? Sku { get; set; }

        public decimal? Price { get; set; }

        public string? ProductId { get; set; }

        public int? StoreId { get; set; }

        public bool? IsBestStore { get; set; }
    }
}
