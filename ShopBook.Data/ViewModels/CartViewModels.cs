using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Data.ViewModels
{
    public class CartViewModels
    {
        public int CartId { get; set; }

        public int? UserId { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
