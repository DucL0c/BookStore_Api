using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Data.ViewModels
{
    public class BookImageViewModels
    {
        public int ImageId { get; set; }

        public int? BookId { get; set; }

        public string? BaseUrl { get; set; }

        public string? SmallUrl { get; set; }

        public string? MediumUrl { get; set; }

        public string? LargeUrl { get; set; }

        public string? ThumbnailUrl { get; set; }

        public bool? IsGallery { get; set; }
    }
}
