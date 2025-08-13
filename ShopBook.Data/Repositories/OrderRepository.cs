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
        Task<Order> CreateOrderAsync(int userId, string shippingAddress, string paymentMethod);
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
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }
        public async Task<List<Order>> GetById(int Id)
        {
            return await _context.Orders
                .Where(o => o.OrderId == Id)
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.Book)//book trong OrderItems
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.Book)//book trong OrderItems
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<Order> CreateOrderAsync(int userId, string shippingAddress, string paymentMethod)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.CartItems.Any())
                throw new Exception("Giỏ hàng trống");

            // Tính tổng tiền
            decimal totalAmount = cart.CartItems.Sum(item => item.Price.GetValueOrDefault() * item.Quantity.GetValueOrDefault());

            // Tạo đơn hàng
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = totalAmount,
                Status = "Pending",
                ShippingAddress = shippingAddress,
                PaymentMethod = paymentMethod
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Tạo OrderItem
            foreach (var cartItem in cart.CartItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.OrderId,
                    BookId = cartItem.BookId,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Price
                };
                _context.OrderItems.Add(orderItem);
            }

            // Xóa cart và cart items
            _context.CartItems.RemoveRange(cart.CartItems);
            _context.Carts.Remove(cart);

            await _context.SaveChangesAsync();

            return order;
        }
    }
}
