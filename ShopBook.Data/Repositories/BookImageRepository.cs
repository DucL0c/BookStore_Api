using Microsoft.EntityFrameworkCore;
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
        Task<List<BookImage>> GetAllByKeyWord(string keyword);
    }
    public class BookImageRepository : RepositoryBase<BookImage>, IBookImageRepository
    {
        private readonly BookstoreContext _context;
        public BookImageRepository(BookstoreContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<BookImage>> GetAllByKeyWord(string keyword)
        {
            return await _context.BookImages
                .Where(ci => string.IsNullOrEmpty(keyword) || ci.Book.Name.Contains(keyword) || ci.Book.Description.Contains(keyword) || ci.Book.ShortDescription.Contains(keyword))
                .Include(bs => bs.Book)
                .ToListAsync();
        }
    }
}
