using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Data.Dto
{
    public class BookDto
    {
        public int BookId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? ShortDescription { get; set; }

        public double? RatingAverage { get; set; }

        public decimal? OriginalPrice { get; set; }

        public decimal? ListPrice { get; set; }
        public QuantitySoldDto QuantitySold { get; set; }  // 👈 thêm dòng này
        public List<BookAuthorDto> BookAuthors { get; set; }
        public List<BookSpecificationDto> BookSpecifications { get; set; }
        public List<BookImageDto> BookImages { get; set; }
        public List<BookSellerDto> BookSellers { get; set; }
        public List<ProductReviewDto> ProductReviews { get; set; }
        public CategoryDto Category { get; set; }
    }
}
