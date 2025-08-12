using Microsoft.EntityFrameworkCore;
using ShopBook.Data.Infrastructure;
using ShopBook.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Data.Repositories
{
    public interface IBookSellerRepository : IRepository<BookSeller>
    {
        Task<List<BookSeller>> GetById(int Id);
        Task<List<BookSeller>> GetByBookIdAsync(int bookId);
        Task<List<BookSeller>> GetBySellerIdAsync(int sellerId);
        Task<List<BookSeller>> GetAllAsync();
        Task<List<BookSeller>> GetAllByKeyWord(string keyword);
    }
    public class BookSellerRepository : RepositoryBase<BookSeller>, IBookSellerRepository
    {
        private readonly BookstoreContext _context;
        public BookSellerRepository(BookstoreContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<BookSeller>> GetAllAsync()
        {
            return await _context.BookSellers
                .Include(bs => bs.Book)
                .Include(bs => bs.Seller)
                .ToListAsync();
        }

        public async Task<List<BookSeller>> GetAllByKeyWord(string keyword)
        {
            return await _context.BookSellers
                .Where(ci => string.IsNullOrEmpty(keyword) || ci.Book.Name.Contains(keyword) || ci.Book.Description.Contains(keyword) || ci.Book.ShortDescription.Contains(keyword))
                .Include(bs => bs.Book)
                .Include(bs => bs.Seller)
                .ToListAsync();
        }

        public async Task<List<BookSeller>> GetByBookIdAsync(int bookId)
        {
            return await _context.BookSellers
             .Where(bs => bs.BookId == bookId)
             .Include(bs => bs.Book)
             .Include(bs => bs.Seller)
             .ToListAsync();
        }

        public async Task<List<BookSeller>> GetById(int Id)
        {
            return await _context.BookSellers
             .Where(bs => bs.Id == Id)
             .Include(bs => bs.Book)
             .Include(bs => bs.Seller)
             .ToListAsync();
        }

        public async Task<List<BookSeller>> GetBySellerIdAsync(int sellerId)
        {
            return await _context.BookSellers
             .Where(bs => bs.SellerId == sellerId)
             .Include(bs => bs.Book)
             .Include(bs => bs.Seller)
             .ToListAsync();
        }
    }
}
