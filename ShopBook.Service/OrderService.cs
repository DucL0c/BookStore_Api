using ShopBook.Data.Dto;
using ShopBook.Data.Models;
using ShopBook.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Service
{
    public interface IOrderService
    {
        Task<List<OrderDetailDto>> GetById(int Id);
        Task<List<OrderDetailDto>> GetByUserIdAsync(int userId);
        Task<List<OrderDetailDto>> GetAllAsyncByKeyWord(string keyWord);
        Task<List<OrderDetailDto>> GetAllAsync();
        Task<Order> Add(Order order);
        Task<Order> Update(Order order);
        Task<Order> Delete(int id);
        Task<Order> CreateOrderAsync(int userId, string shippingAddress, string paymentMethod);
        Task<Order> CreateOrderDirectAsync(int userId, int bookId, int quantity, string shippingAddress, string paymentMethod);

    }
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<Order> Add(Order order)
        {
            return await _orderRepository.AddASync(order);
        }
        public async Task<Order> Delete(int id)
        {
            return await _orderRepository.DeleteAsync(id);
        }
        public async Task<List<OrderDetailDto>> GetAllAsyncByKeyWord(string keyWord)
        {
            return await _orderRepository.GetAllAsyncByKeyWord(keyWord);
        }
        public async Task<List<OrderDetailDto>> GetById(int Id)
        {
            return await _orderRepository.GetById(Id);
        }
        public async Task<List<OrderDetailDto>> GetByUserIdAsync(int userId)
        {
            return await _orderRepository.GetByUserIdAsync(userId);
        }
        public async Task<Order> Update(Order order)
        {
            return await _orderRepository.UpdateASync(order);
        }
        public async Task<List<OrderDetailDto>> GetAllAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        public async Task<Order> CreateOrderAsync(int userId, string shippingAddress, string paymentMethod)
        {
            if (string.IsNullOrWhiteSpace(paymentMethod))
                throw new ArgumentException("PaymentMethod không được để trống.");

            if (string.IsNullOrWhiteSpace(shippingAddress))
                throw new ArgumentException("ShippingAddress không được để trống.");

            return await _orderRepository.CreateOrderAsync(userId, shippingAddress, paymentMethod); 
        }

        public async Task<Order> CreateOrderDirectAsync(int userId, int bookId, int quantity, string shippingAddress, string paymentMethod)
        {
            if (string.IsNullOrWhiteSpace(paymentMethod))
                throw new ArgumentException("PaymentMethod không được để trống.");

            if (string.IsNullOrWhiteSpace(shippingAddress))
                throw new ArgumentException("ShippingAddress không được để trống.");

            return await _orderRepository.CreateOrderDirectAsync(userId,bookId, quantity, shippingAddress, paymentMethod);
        }
    }
}
