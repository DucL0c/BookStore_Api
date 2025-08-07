using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Data.Models
{
    public partial class BookAuthor
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        public int AuthorId { get; set; }

        public virtual Book Book { get; set; } = null!;
        public virtual Author Author { get; set; } = null!;
    }

}
