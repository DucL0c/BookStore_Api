using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Data.Dto
{
    public class ProductReviewDto
    {
        public int ReviewId { get; set; }

        public int? Rating { get; set; }

        public string? Comment { get; set; }

        public DateTime? ReviewDate { get; set; }
        public ReviewUserDto User { get; set; }
    }
}
