using ShopBook.Data.Models;
using ShopBook.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Service
{
    public interface ICartService
    {
        Task<List<Cart>> GetById(int Id);
        Task<List<Cart>> GetByUserIdAsync(int userId);
        Task<List<Cart>> GetAllByKeyWord(string keyWord);
        Task<List<Cart>> GetAllAsync();
        Task<Cart> Add(Cart cart);
        Task<Cart> Update(Cart cart);
        Task<Cart> Delete(int id);

    }
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }
        public async Task<Cart> Add(Cart cart)
        {
            return await _cartRepository.AddASync(cart);
        }
        public async Task<Cart> Delete(int id)
        {
            return await _cartRepository.DeleteAsync(id);
        }
        public async Task<List<Cart>> GetAllByKeyWord(string keyWord)
        {
            return await _cartRepository.GetAllByKeyWord(keyWord);
        }
        public async Task<List<Cart>> GetById(int Id)
        {
            return await _cartRepository.GetById(Id);
        }
        public async Task<List<Cart>> GetByUserIdAsync(int userId)
        {
            return await _cartRepository.GetByUserIdAsync(userId);
        }
        public async Task<Cart> Update(Cart cart)
        {
            return await _cartRepository.UpdateASync(cart);
        }
        public async Task<List<Cart>> GetAllAsync()
        {
            return await _cartRepository.GetAllAsync();
        }
    }
}
