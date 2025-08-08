using ShopBook.Data.Models;
using ShopBook.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Service
{
    public interface ICartItemService
    {
        Task<List<CartItem>> GetById(int Id);
        Task<List<CartItem>> GetByCartIdAsync(int userId);
        Task<List<CartItem>> GetByBookAsync(int userId);
        Task<List<CartItem>> GetAllByKeyWord(string keyWord);
        Task<List<CartItem>> GetAllAsync();
        Task<CartItem> Add(CartItem cartItem);
        Task<CartItem> Update(CartItem cartItem);
        Task<CartItem> Delete(int id);
    }
    public class CartItemService : ICartItemService
    {
        private readonly ICartItemRepository _cartItemRepository;
        public CartItemService(ICartItemRepository cartItemRepository)
        {
            _cartItemRepository = cartItemRepository;
        }
        public async Task<List<CartItem>> GetById(int Id)
        {
            return await _cartItemRepository.GetById(Id);
        }
        public async Task<List<CartItem>> GetByCartIdAsync(int userId)
        {
            return await _cartItemRepository.GetByCartIdAsync(userId);
        }
        public async Task<List<CartItem>> GetByBookAsync(int userId)
        {
            return await _cartItemRepository.GetByBookAsync(userId);
        }
        public async Task<List<CartItem>> GetAllByKeyWord(string keyWord)
        {
            return await _cartItemRepository.GetAllByKeyWord(keyWord);
        }
        public async Task<List<CartItem>> GetAllAsync()
        {
            return await _cartItemRepository.GetAllAsync();
        }
        public async Task<CartItem> Add(CartItem cartItem)
        {
            return await _cartItemRepository.AddASync(cartItem);
        }
        public async Task<CartItem> Update(CartItem cartItem)
        {
            return await _cartItemRepository.UpdateASync(cartItem);
        }
        public async Task<CartItem> Delete(int id)
        {
            return await _cartItemRepository.DeleteAsync(id);
        }
    }
}
