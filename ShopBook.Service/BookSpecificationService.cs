using ShopBook.Data.Models;
using ShopBook.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Service
{
    public interface IBookSpecificationService
    {
        Task<List<BookSpecification>> GetById(int Id);
        Task<List<BookSpecification>> GetByBookIdAsync(int bookId);
        Task<List<BookSpecification>> GetAllAsync();
        Task<List<BookSpecification>> GetAllByKeyWord(string keyWord);
        Task<BookSpecification> Add(BookSpecification specification);
        Task<BookSpecification> Update(BookSpecification specification);
        Task<BookSpecification> Delete(int id);
    }
    public class BookSpecificationService : IBookSpecificationService
    {
        private readonly IBookSpecificationRepository _bookSpecificationRepository;
        public BookSpecificationService(IBookSpecificationRepository bookSpecificationRepository)
        {
            _bookSpecificationRepository = bookSpecificationRepository;
        }
        public async Task<BookSpecification> Add(BookSpecification specification)
        {
            return await _bookSpecificationRepository.AddASync(specification);
        }
        public async Task<BookSpecification> Delete(int id)
        {
            return await _bookSpecificationRepository.DeleteAsync(id);
        }
        public async Task<List<BookSpecification>> GetAllAsync()
        {
            return await _bookSpecificationRepository.GetAllAsync();
        }

        public async Task<List<BookSpecification>> GetAllByKeyWord(string keyWord)
        {
           return await _bookSpecificationRepository.GetAllByKeyWord(keyWord);
        }

        public async Task<List<BookSpecification>> GetByBookIdAsync(int bookId)
        {
            return await _bookSpecificationRepository.GetByBookIdAsync(bookId);
        }
        public async Task<List<BookSpecification>> GetById(int Id)
        {
            return await _bookSpecificationRepository.GetById(Id);
        }
        public async Task<BookSpecification> Update(BookSpecification specification)
        {
            return await _bookSpecificationRepository.UpdateASync(specification);
        }
    }
}
