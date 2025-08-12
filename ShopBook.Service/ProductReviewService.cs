using ShopBook.Data.Models;
using ShopBook.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Service
{
    public interface IProductReviewService
    {
        Task<List<ProductReview>> GetById(int Id);
        Task<List<ProductReview>> GetByBookIdAsync(int bookId);
        Task<List<ProductReview>> GetByUserIdAsync(int userId);
        Task<List<ProductReview>> GetAllAsync();
        Task<List<ProductReview>> GetAllByKeyWord(string keyWord);
        Task<ProductReview> Add(ProductReview productReview);
        Task<ProductReview> Update(ProductReview productReview);
        Task<ProductReview> Delete(int id);
    }
    public class ProductReviewService : IProductReviewService
    {
        private readonly IProductReviewRepository _productReviewRepository;
        public ProductReviewService(IProductReviewRepository productReviewRepository)
        {
            _productReviewRepository = productReviewRepository;
        }
        public async Task<ProductReview> Add(ProductReview productReview)
        {
            return await _productReviewRepository.AddASync(productReview);
        }
        public async Task<ProductReview> Delete(int id)
        {
            return await _productReviewRepository.DeleteAsync(id);
        }
        public async Task<List<ProductReview>> GetAllAsync()
        {
            return await _productReviewRepository.GetAllAsync();
        }

        public async Task<List<ProductReview>> GetAllByKeyWord(string keyWord)
        {
           return await _productReviewRepository.GetAllByKeyWord(keyWord);
        }

        public async Task<List<ProductReview>> GetByBookIdAsync(int bookId)
        {
            return await _productReviewRepository.GetByBookIdAsync(bookId);
        }
        public async Task<List<ProductReview>> GetById(int Id)
        {
            return await _productReviewRepository.GetById(Id);
        }

        public async Task<List<ProductReview>> GetByUserIdAsync(int userId)
        {
            return await _productReviewRepository.GetByUserIdAsync(userId);
        }

        public async Task<ProductReview> Update(ProductReview productReview)
        {
            return await _productReviewRepository.UpdateASync(productReview);
        }
    }
}
