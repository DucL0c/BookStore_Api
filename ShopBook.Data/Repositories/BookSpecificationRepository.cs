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
    public interface IBookSpecificationRepository : IRepository<BookSpecification>
    {
        Task<List<BookSpecification>> GetById(int Id);
        Task<List<BookSpecification>> GetByBookIdAsync(int bookId);
        Task<List<BookSpecification>> GetAllAsync();
        Task<List<BookSpecification>> GetAllByKeyWord(string keyWord);
    }

    public class BookSpecificationRepository : RepositoryBase<BookSpecification>, IBookSpecificationRepository
    {
        private readonly BookstoreContext _context;
        public BookSpecificationRepository(BookstoreContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<BookSpecification>> GetAllAsync()
        {
            return await _context.BookSpecifications
                .Include(bs => bs.Book)
                .ToListAsync();
        }

        public async Task<List<BookSpecification>> GetAllByKeyWord(string keyWord)
        {
            return await _context.BookSpecifications
                .Where(bs => string.IsNullOrEmpty(keyWord) || bs.Book.Name.Contains(keyWord) || bs.Book.Description.Contains(keyWord))
                .Include(bs => bs.Book)
                .ToListAsync();
        }

        public async Task<List<BookSpecification>> GetByBookIdAsync(int bookId)
        {
            return await _context.BookSpecifications
             .Where(bs => bs.BookId == bookId)
             .Include(bs => bs.Book)
             .ToListAsync();
        }

        public async Task<List<BookSpecification>> GetById(int Id)
        {
            return await _context.BookSpecifications
             .Where(bs => bs.Id == Id)
             .Include(bs => bs.Book)
             .ToListAsync();
        }
    }
}
