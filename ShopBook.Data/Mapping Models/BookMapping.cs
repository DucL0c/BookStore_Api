using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Data.Mapping_Models
{
    public class QuantitySoldModel
    {
        public int? Value { get; set; }
        public string Text { get; set; }
    }
    public class BookMapping
    {
        public int BookId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? ShortDescription { get; set; }

        public double? RatingAverage { get; set; }

        public decimal? OriginalPrice { get; set; }

        public decimal? ListPrice { get; set; }

        public QuantitySoldModel QuantitySold { get; set; }  // Object gồm value + text

        public int? CategoryId { get; set; }
    }

}
