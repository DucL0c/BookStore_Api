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
    public interface IBookAuthorRepository : IRepository<BookAuthor>
    {
        Task<List<BookAuthor>> GetById(int Id);
        Task<List<BookAuthor>> GetByBookIdAsync(int bookId);
        Task<List<BookAuthor>> GetBySellerIdAsync(int authorId);
        Task<List<BookAuthor>> GetAllAsync();
    }
    public class BookAuthorRepository : RepositoryBase<BookAuthor>, IBookAuthorRepository
    {
        private readonly BookstoreContext _context;
        public BookAuthorRepository(BookstoreContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<BookAuthor>> GetAllAsync()
        {
            return await _context.BookAuthors
                .Include(ba => ba.Book)
                .Include(ba => ba.Author)
                .ToListAsync();
        }
        public async Task<List<BookAuthor>> GetByBookIdAsync(int bookId)
        {
            return await _context.BookAuthors
                .Where(ba => ba.BookId == bookId)
                .Include(ba => ba.Book)
                .Include(ba => ba.Author)
                .ToListAsync();
        }
        public async Task<List<BookAuthor>> GetById(int Id)
        {
            return await _context.BookAuthors
                .Where(ba => ba.Id == Id)
                .Include(ba => ba.Book)
                .Include(ba => ba.Author)
                .ToListAsync();
        }
        public async Task<List<BookAuthor>> GetBySellerIdAsync(int authorId)
        {
            return await _context.BookAuthors
                .Where(ba => ba.AuthorId == authorId) 
                .Include(ba => ba.Book)
                .Include(ba => ba.Author)
                .ToListAsync();
        }
    }
}
