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
                .Where(c => string.IsNullOrEmpty(keyWord) || c.User.FullName.ToString().Contains(keyWord) || c.User.Email.ToString().Contains(keyWord))
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
    }
}
