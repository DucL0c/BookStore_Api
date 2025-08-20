using Microsoft.EntityFrameworkCore;
using ShopBook.Data.Dto;
using ShopBook.Data.Infrastructure;
using ShopBook.Data.Models;
using ShopBook.Model.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Data.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<List<OrderDetailDto>> GetById(int Id);
        Task<List<OrderDetailDto>> GetByUserIdAsync(int userId);
        Task<List<OrderDetailDto>> GetAllAsyncByKeyWord(string keyWord);
        Task<List<OrderDetailDto>> GetAllAsync();
        Task<Order> CreateOrderAsync(int userId, string shippingAddress, string paymentMethod);
        Task<Order> CreateOrderDirectAsync(int userId, int bookId, int quantity, string shippingAddress, string paymentMethod);
    }
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        private readonly BookstoreContext _context;
        public OrderRepository(BookstoreContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<OrderDetailDto>> GetAllAsyncByKeyWord(string keyword)
        {
            var query = _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems).ThenInclude(o=>o.Book).ThenInclude(o=>o.BookImages)
                .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(o =>
                    o.ShippingAddress.Contains(keyword) ||
                    o.User.Name.Contains(keyword) // Tìm theo tên người dùng
                );
            }
            var orders = await query.ToListAsync();

            var result = orders.Select(o => new OrderDetailDto
            {
                OrderId = o.OrderId,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                ShippingAddress = o.ShippingAddress,
                PaymentMethod = o.PaymentMethod,
                User = o.User == null ? null : new UsersDto
                {
                    UserId = o.User.UserId,
                    Name = o.User.Name,
                    Phone = o.User.Phone,
                    Address = o.User.Address
                },
                OrderItems = o.OrderItems.Select(oi => new OrderItemDto
                {
                    OrderItemId = oi.OrderItemId,
                    Quantity = oi.Quantity,
                    Price = oi.Price,
                    Book = oi.Book == null ? null : new BooksDto
                    {
                        BookId = oi.Book.BookId,
                        Name = oi.Book.Name,
                        Price = oi.Book.ListPrice,
                        BookImages = oi.Book.BookImages.Select(bi => new BookImageOrderDto
                        {
                            ImageId = bi.ImageId,
                            BaseUrl = bi.BaseUrl,
                            SmallUrl = bi.SmallUrl,
                            MediumUrl = bi.MediumUrl,
                            LargeUrl = bi.LargeUrl,
                            ThumbnailUrl = bi.LargeUrl
                        }).ToList()
                    }
                }).ToList()
            }).ToList();

            return result;

        }

        public async Task<List<OrderDetailDto>> GetByUserIdAsync(int userId)
        {
            var query = _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.Book)
                .ThenInclude(o => o.BookImages)
                .OrderByDescending(o => o.OrderDate);

            var orders = await query.ToListAsync();

            var result = orders.Select(o => new OrderDetailDto
            {
                OrderId = o.OrderId,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                ShippingAddress = o.ShippingAddress,
                PaymentMethod = o.PaymentMethod,
                User = o.User == null ? null : new UsersDto
                {
                    UserId = o.User.UserId,
                    Name = o.User.Name,
                    Phone = o.User.Phone,
                    Address = o.User.Address
                },
                OrderItems = o.OrderItems.Select(oi => new OrderItemDto
                {
                    OrderItemId = oi.OrderItemId,
                    Quantity = oi.Quantity,
                    Price = oi.Price,
                    Book = oi.Book == null ? null : new BooksDto
                    {
                        BookId = oi.Book.BookId,
                        Name = oi.Book.Name,
                        Price = oi.Book.ListPrice,
                        BookImages = oi.Book.BookImages.Select(bi => new BookImageOrderDto
                        {
                            ImageId = bi.ImageId,
                            BaseUrl = bi.BaseUrl,
                            SmallUrl = bi.SmallUrl,
                            MediumUrl = bi.MediumUrl,
                            LargeUrl = bi.LargeUrl,
                            ThumbnailUrl = bi.LargeUrl
                        }).ToList()
                    }
                }).ToList()
            }).ToList();

            return result;
        }
        public async Task<List<OrderDetailDto>> GetById(int Id)
        {
            var query = _context.Orders
                .Where(o => o.OrderId == Id)
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.Book)
                .ThenInclude(o => o.BookImages)
                .OrderByDescending(o => o.OrderDate);

            var orders = await query.ToListAsync();

            var result = orders.Select(o => new OrderDetailDto
            {
                OrderId = o.OrderId,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                ShippingAddress = o.ShippingAddress,
                PaymentMethod = o.PaymentMethod,
                User = o.User == null ? null : new UsersDto
                {
                    UserId = o.User.UserId,
                    Name = o.User.Name,
                    Phone = o.User.Phone,
                    Address = o.User.Address
                },
                OrderItems = o.OrderItems.Select(oi => new OrderItemDto
                {
                    OrderItemId = oi.OrderItemId,
                    Quantity = oi.Quantity,
                    Price = oi.Price,
                    Book = oi.Book == null ? null : new BooksDto
                    {
                        BookId = oi.Book.BookId,
                        Name = oi.Book.Name,
                        Price = oi.Book.ListPrice,
                        BookImages = oi.Book.BookImages.Select(bi => new BookImageOrderDto
                        {
                            ImageId = bi.ImageId,
                            BaseUrl = bi.BaseUrl,
                            SmallUrl = bi.SmallUrl,
                            MediumUrl = bi.MediumUrl,
                            LargeUrl = bi.LargeUrl,
                            ThumbnailUrl = bi.LargeUrl
                        }).ToList()
                    }
                }).ToList()
            }).ToList();

            return result;
        }

        public async Task<List<OrderDetailDto>> GetAllAsync()
        {
            var query = _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.Book)
                .ThenInclude(o => o.BookImages)
                .OrderByDescending(o => o.OrderDate);

            var orders = await query.ToListAsync();

            var result = orders.Select(o => new OrderDetailDto
            {
                OrderId = o.OrderId,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                ShippingAddress = o.ShippingAddress,
                PaymentMethod = o.PaymentMethod,
                User = o.User == null ? null : new UsersDto
                {
                    UserId = o.User.UserId,
                    Name = o.User.Name,
                    Phone = o.User.Phone,
                    Address = o.User.Address
                },
                OrderItems = o.OrderItems.Select(oi => new OrderItemDto
                {
                    OrderItemId = oi.OrderItemId,
                    Quantity = oi.Quantity,
                    Price = oi.Price,
                    Book = oi.Book == null ? null : new BooksDto
                    {
                        BookId = oi.Book.BookId,
                        Name = oi.Book.Name,
                        Price = oi.Book.ListPrice,
                        BookImages = oi.Book.BookImages.Select(bi => new BookImageOrderDto
                        {
                            ImageId = bi.ImageId,
                            BaseUrl = bi.BaseUrl,
                            SmallUrl = bi.SmallUrl,
                            MediumUrl = bi.MediumUrl,
                            LargeUrl = bi.LargeUrl,
                            ThumbnailUrl = bi.LargeUrl
                        }).ToList()
                    }
                }).ToList()
            }).ToList();

            return result;
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
                Status = "pending",
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

        public async Task<Order> CreateOrderDirectAsync(int userId, int bookId, int quantity, string shippingAddress, string paymentMethod)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
                throw new Exception("Sách không tồn tại");

            if (quantity <= 0)
                throw new Exception("Số lượng không hợp lệ");

            // Tính tổng tiền
            decimal totalAmount = (book.ListPrice ?? 0) * quantity;

            // Tạo order
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = totalAmount,
                Status = "pending",
                ShippingAddress = shippingAddress,
                PaymentMethod = paymentMethod,
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Tạo order item
            var orderItem = new OrderItem
            {
                OrderId = order.OrderId,
                BookId = bookId,
                Quantity = quantity,
                Price = book.ListPrice
            };
            _context.OrderItems.Add(orderItem);

            await _context.SaveChangesAsync();

            return order;
        }
    }
}
