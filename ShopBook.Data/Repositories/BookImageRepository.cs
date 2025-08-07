using ShopBook.Data.Infrastructure;
using ShopBook.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Data.Repositories
{
    public interface IBookImageRepository : IRepository<BookImage>
    {
    }
    public class BookImageRepository : RepositoryBase<BookImage>, IBookImageRepository
    {
        private readonly BookstoreContext _context;
        public BookImageRepository(BookstoreContext context) : base(context)
        {
            _context = context;
        }
    }
}
