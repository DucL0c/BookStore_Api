using ShopBook.Data.Models;
using ShopBook.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Service
{
    public interface IBookSellerService
    {
        Task<List<BookSeller>> GetAllAsync();
        Task<List<BookSeller>> GetAllByKeyWord(string keyword);
        Task<List<BookSeller>> GetByBookIdAsync(int bookId);
        Task<List<BookSeller>> GetBySellerIdAsync(int sellerId);
        Task<BookSeller> Add(BookSeller bookSeller);
        Task<List<BookSeller>> GetById(int Id);
        Task<BookSeller> Update(BookSeller bookSeller);
        Task<BookSeller> Delete(int id);
    }
    public class BookSellerService : IBookSellerService
    {
        private readonly IBookSellerRepository _bookSellerRepository;
        public BookSellerService(IBookSellerRepository bookSellerRepository)
        {
            _bookSellerRepository = bookSellerRepository;
        }

        public async Task<BookSeller> Add(BookSeller bookSeller)
        {
           return await _bookSellerRepository.AddASync(bookSeller);
        }

        public async Task<BookSeller> Delete(int id)
        {
            return await _bookSellerRepository.DeleteAsync(id);
        }

        public async Task<List<BookSeller>> GetAllAsync()
        {
            return await _bookSellerRepository.GetAllAsync();
        }

        public async Task<List<BookSeller>> GetAllByKeyWord(string keyword)
        {
           return await _bookSellerRepository.GetAllByKeyWord(keyword);
        }

        public async Task<List<BookSeller>> GetByBookIdAsync(int bookId)
        {
            return await _bookSellerRepository.GetByBookIdAsync(bookId);
        }

        public async Task<List<BookSeller>> GetById(int Id)
        {
            return await _bookSellerRepository.GetById(Id);
        }

        public async Task<List<BookSeller>> GetBySellerIdAsync(int sellerId)
        {
            return await _bookSellerRepository.GetBySellerIdAsync(sellerId);
        }

        public async Task<BookSeller> Update(BookSeller bookSeller)
        {
           return await _bookSellerRepository.UpdateASync(bookSeller);
        }
    }
}
