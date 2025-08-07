using ShopBook.Data.Models;
using ShopBook.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Service
{
    public interface IOrderItemService
    {
        Task<List<OrderItem>> GetById(int Id);
        Task<List<OrderItem>> GetByOrderIdAsync(int orderId);
        Task<List<OrderItem>> GetByBookIdAsync(int bookId);
        Task<List<OrderItem>> GetAllAsync(string keyWord);
        Task<OrderItem> Add(OrderItem orderItem);
        Task<OrderItem> Update(OrderItem orderItem);
        Task<OrderItem> Delete(int id);

    }
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;
        public OrderItemService(IOrderItemRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }
        public async Task<OrderItem> Add(OrderItem orderItem)
        {
            return await _orderItemRepository.AddASync(orderItem);
        }
        public async Task<OrderItem> Delete(int id)
        {
            return await _orderItemRepository.DeleteAsync(id);
        }
        public async Task<List<OrderItem>> GetAllAsync(string keyWord)
        {
            return await _orderItemRepository.GetAllAsync(keyWord);
        }
        public async Task<List<OrderItem>> GetByBookIdAsync(int bookId)
        {
            return await _orderItemRepository.GetByBookIdAsync(bookId);
        }
        public async Task<List<OrderItem>> GetById(int Id)
        {
            return await _orderItemRepository.GetById(Id);
        }
        public async Task<List<OrderItem>> GetByOrderIdAsync(int orderId)
        {
            return await _orderItemRepository.GetByOrderIdAsync(orderId);
        }
        public async Task<OrderItem> Update(OrderItem orderItem)
        {
            return await _orderItemRepository.UpdateASync(orderItem);
        }

    }
}
