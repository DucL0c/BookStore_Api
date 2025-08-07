using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Data.ViewModels
{
    public class CategoryViewModels
    {
        public int CategoryId { get; set; }

        public string? Name { get; set; }

        public bool? IsLeaf { get; set; }
    }
}
