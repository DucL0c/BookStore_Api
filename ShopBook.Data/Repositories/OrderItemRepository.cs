using Microsoft.EntityFrameworkCore;
using ShopBook.Data.Infrastructure;
using ShopBook.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ShopBook.Data.Repositories
{
    public interface IOrderItemRepository : IRepository<OrderItem>
    {
        Task<List<OrderItem>> GetById(int Id);
        Task<List<OrderItem>> GetByOrderIdAsync(int orderId);
        Task<List<OrderItem>> GetByBookIdAsync(int bookId);
        Task<List<OrderItem>> GetAllAsync(string keyWord);
    }
    public class OrderItemRepository : RepositoryBase<OrderItem>, IOrderItemRepository
    {
        private readonly BookstoreContext _context;
        public OrderItemRepository(BookstoreContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<OrderItem>> GetAllAsync(string keyWord)
        {
           var query = _context.OrderItems
                .Include(oi => oi.Book)
                .Include(oi => oi.Order)
                 .AsQueryable();

            if (!string.IsNullOrEmpty(keyWord))
            {
                query = query.Where(o =>
                    o.Book.Name.Contains(keyWord) ||
                    o.Book.Description.Contains(keyWord) ||
                    o.Book.ShortDescription.Contains(keyWord) ||
                    o.Order.ShippingAddress.Contains(keyWord) // Tìm theo tên người dùng
                );
            }
            return await query.ToListAsync();

        }
        public async Task<List<OrderItem>> GetByBookIdAsync(int bookId)
        {
            return await _context.OrderItems
                .Where(oi => oi.BookId == bookId)
                .Include(oi => oi.Book)
                .Include(oi => oi.Order)
                .ToListAsync();
        }
        public async Task<List<OrderItem>> GetById(int Id)
        {
            return await _context.OrderItems
                .Where(oi => oi.OrderId == Id)
                .Include(oi => oi.Book)
                .Include(oi => oi.Order)
                .ToListAsync();
        }
        public async Task<List<OrderItem>> GetByOrderIdAsync(int orderId)
        {
            return await _context.OrderItems
                .Where(oi => oi.OrderId == orderId)
                .Include(oi => oi.Book)
                .Include(oi => oi.Order)
                .ToListAsync();
        }
    }
}
