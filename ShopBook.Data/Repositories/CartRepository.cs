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
    public interface ICartRepository : IRepository<Cart>
    {
        Task<List<Cart>> GetById(int Id);
        Task<List<Cart>> GetByUserIdAsync(int userId);
        Task<List<Cart>> GetAllByKeyWord(string keyWord);
        Task<List<Cart>> GetAllAsync();

        Task<Cart> AddToCartAsync(int userId, int bookId, int quantity);

    }
    public class CartRepository : RepositoryBase <Cart> , ICartRepository
    {
        private readonly BookstoreContext _context;
        public CartRepository(BookstoreContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<Cart>> GetByUserIdAsync(int userId)
        {
            return await _context.Carts
                .Where(c => c.UserId == userId)
                .Include(c => c.User)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Book) // Include Book trong CartItem
                .ToListAsync();
        }
        public async Task<List<Cart>> GetAllByKeyWord(string keyWord)
        {
            return await _context.Carts
                .Where(c => string.IsNullOrEmpty(keyWord) || c.User.Name.ToString().Contains(keyWord) || c.User.Email.ToString().Contains(keyWord))
                .Include(c => c.User)
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Book) // Include Book trong CartItem
                .ToListAsync();
        }

        public async Task<List<Cart>> GetById(int Id)
        {
            return await _context.Carts
                .Where(c => c.CartId == Id)
                .Include(c => c.User)
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Book) // Include Book trong CartItem
                .ToListAsync();
        }

        public async Task<List<Cart>> GetAllAsync()
        {
            return await _context.Carts
               .Include(c => c.User)
               .Include(c => c.CartItems)
               .ThenInclude(ci => ci.Book) // Include Book trong CartItem
               .ToListAsync();
        }

        public async Task<Cart> AddToCartAsync(int userId, int bookId, int quantity)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null) throw new Exception("Book not found");

            // Retrieve the user's cart
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                };
                _context.Carts.Add(cart);
            }

            // Check if the item already exists
            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.BookId == bookId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                existingItem.Price = existingItem.Quantity * book.ListPrice;
            }
            else
            {
                cart.CartItems.Add(new CartItem
                {
                    BookId = bookId,
                    Quantity = quantity,
                    Price = book.ListPrice * quantity
                });
            }

            await _context.SaveChangesAsync(); // Save changes to the database
            return cart; // Return the updated cart
        }
    }
}
