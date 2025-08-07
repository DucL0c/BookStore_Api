using Microsoft.EntityFrameworkCore;
using ShopBook.Data.Infrastructure;
using ShopBook.Data.Mapping_Models;
using ShopBook.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Data.Repositories
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<List<BookMapping>> GetAllBookMapping(string keyWord);
    }
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        private readonly BookstoreContext _context;
        public BookRepository(BookstoreContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<BookMapping>> GetAllBookMapping(string keyWord)
        {
            var rawBooksQuery = _context.Books.AsQueryable();

            if (!string.IsNullOrEmpty(keyWord))
            {
                rawBooksQuery = rawBooksQuery
                    .Where(b => b.Name.ToLower().Contains(keyWord.ToLower()));
            }
            var rawBooks = await rawBooksQuery
             .Select(b => new
             {
                 b.BookId,
                 b.Name,
                 b.Description,
                 b.ShortDescription,
                 b.RatingAverage,
                 b.OriginalPrice,
                 b.ListPrice,
                 b.QuantitySold,
                 b.CategoryId
             })
             .ToListAsync();

            var books = rawBooks.Select(b => new BookMapping
            {
                BookId = b.BookId,
                Name = b.Name,
                Description = b.Description,
                ShortDescription = b.ShortDescription,
                RatingAverage = b.RatingAverage,
                OriginalPrice = b.OriginalPrice,
                ListPrice = b.ListPrice,
                CategoryId = b.CategoryId,
                QuantitySold = new QuantitySoldModel
                {
                    Value = b.QuantitySold,
                    Text = b.QuantitySold.HasValue
                        ? $"Đã bán {(b.QuantitySold >= 1000 ? "hơn 1000+" : b.QuantitySold.Value.ToString())}"
                        : null
                }
            }).ToList();
            return books;
        }
    }
}
