using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Data.ViewModels
{
    public class BookSpecificationViewModels
    {
        public int Id { get; set; }
        public int BookId { get; set; }

        public string SpecName { get; set; } = null!;

        public string? SpecValue { get; set; }
        public string SpecCode { get; set; } = null!;
    }
}
