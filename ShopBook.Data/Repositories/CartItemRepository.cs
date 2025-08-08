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
    public interface ICartItemRepository : IRepository<CartItem>
    {
        Task<List<CartItem>> GetById(int Id);
        Task<List<CartItem>> GetByCartIdAsync(int userId);
        Task<List<CartItem>> GetByBookAsync(int userId);
        Task<List<CartItem>> GetAllByKeyWord(string keyWord);
        Task<List<CartItem>> GetAllAsync();
    }
    public class CartItemRepository : RepositoryBase<CartItem>, ICartItemRepository
    {
        private readonly BookstoreContext _context;
        public CartItemRepository(BookstoreContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<CartItem>> GetByCartIdAsync(int cartId)
        {
            return await _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .Include(ci => ci.Cart)
                .Include(ci => ci.Book)
                .ToListAsync();
        }
        public async Task<List<CartItem>> GetByBookAsync(int bookId)
        {
            return await _context.CartItems
                .Where(ci => ci.BookId == bookId)
                .Include(ci => ci.Cart)
                .Include(ci => ci.Book)
                .ToListAsync();
        }
        public async Task<List<CartItem>> GetAllByKeyWord(string keyWord)
        {
            return await _context.CartItems
                .Where(ci => string.IsNullOrEmpty(keyWord) || ci.Book.Name.Contains(keyWord) || ci.Book.Description.Contains(keyWord) || ci.Book.ShortDescription.Contains(keyWord))
                .Include(ci => ci.Cart)
                .Include(ci => ci.Book)
                .ToListAsync();
        }
        public async Task<List<CartItem>> GetById(int Id)
        {
            return await _context.CartItems
                .Where(ci => ci.CartItemId == Id)
                .Include(ci => ci.Cart)
                .Include(ci => ci.Book)
                .ToListAsync();
        }
        public async Task<List<CartItem>> GetAllAsync()
        {
            return await _context.CartItems
                .Include(ci => ci.Cart)
               .Include(ci => ci.Book)
               .ToListAsync();
        }
    }
}
