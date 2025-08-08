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
    public interface IOrderRepository : IRepository<Order>
    {
        Task<List<Order>> GetById(int Id);
        Task<List<Order>> GetByUserIdAsync(int userId);
        Task<List<Order>> GetAllAsyncByKeyWord(string keyWord);
        Task<List<Order>> GetAllAsync();
    }
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        private readonly BookstoreContext _context;
        public OrderRepository(BookstoreContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<Order>> GetAllAsyncByKeyWord(string keyword)
        {
            var query = _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(o=>o.Book)//book trong OrderItems
                .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(o =>
                    o.ShippingAddress.Contains(keyword) ||
                    o.User.FullName.Contains(keyword) // Tìm theo tên người dùng
                );
            }
            return await query.ToListAsync();
        }

        public async Task<List<Order>> GetByUserIdAsync(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.Book)//book trong OrderItems
                .ToListAsync();
        }
        public async Task<List<Order>> GetById(int Id)
        {
            return await _context.Orders
                .Where(o => o.OrderId == Id)
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.Book)//book trong OrderItems
                .ToListAsync();
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.Book)//book trong OrderItems
                .ToListAsync();
        }
    }
}
