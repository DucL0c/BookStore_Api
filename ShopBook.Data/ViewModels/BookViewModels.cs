using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Data.ViewModels
{
    public class BookViewModels
    {
        public int BookId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? ShortDescription { get; set; }

        public double? RatingAverage { get; set; }

        public decimal? OriginalPrice { get; set; }

        public decimal? ListPrice { get; set; }

        public int? QuantitySold { get; set; }

        public int? CategoryId { get; set; }
    }
}
