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
        Task<List<Order>> GetById(int Id);
        Task<List<Order>> GetByUserIdAsync(int userId);
        Task<List<Order>> GetAllAsyncByKeyWord(string keyWord);
        Task<List<Order>> GetAllAsync();
        Task<Order> Add(Order order);
        Task<Order> Update(Order order);
        Task<Order> Delete(int id);

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
        public async Task<List<Order>> GetAllAsyncByKeyWord(string keyWord)
        {
            return await _orderRepository.GetAllAsyncByKeyWord(keyWord);
        }
        public async Task<List<Order>> GetById(int Id)
        {
            return await _orderRepository.GetById(Id);
        }
        public async Task<List<Order>> GetByUserIdAsync(int userId)
        {
            return await _orderRepository.GetByUserIdAsync(userId);
        }
        public async Task<Order> Update(Order order)
        {
            return await _orderRepository.UpdateASync(order);
        }
        public async Task<List<Order>> GetAllAsync()
        {
            return await _orderRepository.GetAllAsync();
        }
    }
}
