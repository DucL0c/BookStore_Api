using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Data.ViewModels
{
    public class ProductReviewViewModels
    {
        public int ReviewId { get; set; }

        public int? UserId { get; set; }

        public int? BookId { get; set; }

        public int? Rating { get; set; }

        public string? Comment { get; set; }

        public DateTime? ReviewDate { get; set; }
    }
}
