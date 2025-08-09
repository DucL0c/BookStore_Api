using ShopBook.Data.Dto;
using ShopBook.Data.Models;
using ShopBook.Data.Repositories;

namespace ShopBook.Service
{
    public interface IBookService
    {
        Task<List<BookDto>> GetAllByKeyWord(string keyWord, int? categoryId);
        Task<List<BookDto>> GetAll();
        Task<BookDto?> GetBookByIdAsync(int bookId);
        Task<Book> Add(Book book);
        Task<Book> Update(Book book);
        Task<Book> Delete(int id);
    }
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        public async Task<Book> Add(Book book)
        {
            return await _bookRepository.AddASync(book);
        }
        public async Task<Book> Delete(int id)
        {
            return await _bookRepository.DeleteAsync(id);
        }

        public async Task<List<BookDto>> GetAll()
        {
            return await _bookRepository.GetAll();
        }

        public async Task<List<BookDto>> GetAllByKeyWord(string keyWord, int? categoryId)
        {
            return await _bookRepository.GetAllByKeyWord(keyWord,categoryId);
        }

        public async Task<BookDto?> GetBookByIdAsync(int bookId)
        {
           return await _bookRepository.GetBookByIdAsync(bookId);
        }

        public async Task<Book> Update(Book book)
        {
            return await _bookRepository.UpdateASync(book);
        }
    }

}
