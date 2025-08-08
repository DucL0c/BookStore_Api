using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Data.Dto
{
    public class BookAuthorDto
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        public int AuthorId { get; set; }
        public AuthorDto Author { get; set; }
    }
}
