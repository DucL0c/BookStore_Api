using ShopBook.Data.Models;
using ShopBook.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Service
{
    public interface IBookAuthorService
    {
        Task<List<BookAuthor>> GetById(int Id);
        Task<List<BookAuthor>> GetByBookIdAsync(int bookId);
        Task<List<BookAuthor>> GetBySellerIdAsync(int authorId);
        Task<List<BookAuthor>> GetAllAsync();
        Task<List<BookAuthor>> GetAllByKeyWord(string keyWord);
        Task<BookAuthor> Add(BookAuthor bookAuthor);
        Task<BookAuthor> Update(BookAuthor bookAuthor);
        Task<BookAuthor> Delete(int id);
    }
    public class BookAuthorService : IBookAuthorService
    {
        private readonly IBookAuthorRepository _bookAuthorRepository;
        public BookAuthorService(IBookAuthorRepository bookAuthorRepository)
        {
            _bookAuthorRepository = bookAuthorRepository;
        }

        public async Task<BookAuthor> Add(BookAuthor bookAuthor)
        {
           return await _bookAuthorRepository.AddASync(bookAuthor);
        }

        public async Task<BookAuthor> Delete(int id)
        {
            return await _bookAuthorRepository.DeleteAsync(id);
        }

        public async Task<List<BookAuthor>> GetAllAsync()
        {
            return await _bookAuthorRepository.GetAllAsync();
        }

        public async Task<List<BookAuthor>> GetAllByKeyWord(string keyWord)
        {
            return await _bookAuthorRepository.GetAllByKeyWord(keyWord);
        }

        public async Task<List<BookAuthor>> GetByBookIdAsync(int bookId)
        {
            return await _bookAuthorRepository.GetByBookIdAsync(bookId);
        }

        public async Task<List<BookAuthor>> GetById(int Id)
        {
            return await _bookAuthorRepository.GetById(Id);
        }

        public async Task<List<BookAuthor>> GetBySellerIdAsync(int authorId)
        {
            return await _bookAuthorRepository.GetBySellerIdAsync(authorId);
        }

        public async Task<BookAuthor> Update(BookAuthor bookAuthor)
        {
            return await _bookAuthorRepository.UpdateASync(bookAuthor);
        }
    }
}
