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
    public interface IProductReviewRepository : IRepository<ProductReview>
    {
        Task<List<ProductReview>> GetById(int Id);
        Task<List<ProductReview>> GetByBookIdAsync(int bookId);
        Task<List<ProductReview>> GetByUserIdAsync(int userId);
        Task<List<ProductReview>> GetAllAsync();
        Task<List<ProductReview>> GetAllByKeyWord(string keyWord);
    }
    public class ProductReviewRepository : RepositoryBase<ProductReview>, IProductReviewRepository
    {
        public readonly BookstoreContext _context;
        public ProductReviewRepository(BookstoreContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<ProductReview>> GetAllAsync()
        {
            return await _context.ProductReviews
                .Include(pr => pr.Book)
                .Include(pr => pr.User)
                .ToListAsync();
        }

        public Task<List<ProductReview>> GetAllByKeyWord(string keyWord)
        {
            return _context.ProductReviews
                .Where(pr => string.IsNullOrEmpty(keyWord) 
                || pr.Book.Name.Contains(keyWord) 
                || pr.Book.Description.Contains(keyWord) 
                || pr.Book.ShortDescription.Contains(keyWord) 
                || pr.User.Name.Contains(keyWord)
                || pr.User.Email.Contains(keyWord))
                .Include(pr => pr.Book)
                .Include(pr => pr.User)
                .ToListAsync();
        }

        public async Task<List<ProductReview>> GetByBookIdAsync(int bookId)
        {
            return await _context.ProductReviews
                .Where(pr => pr.BookId == bookId)
                .Include(pr => pr.Book)
                .Include(pr => pr.User)
                .ToListAsync();
        }
        public async Task<List<ProductReview>> GetById(int Id)
        {
            return await _context.ProductReviews
                .Where(pr => pr.ReviewId == Id)
                .Include(pr => pr.Book)
                .Include(pr => pr.User)
                .ToListAsync();
        }

        public async Task<List<ProductReview>> GetByUserIdAsync(int userId)
        {
            return await _context.ProductReviews
                .Where(pr => pr.UserId == userId)
                .Include(pr => pr.Book)
                .Include(pr => pr.User)
                .ToListAsync();
        }
    }
}
