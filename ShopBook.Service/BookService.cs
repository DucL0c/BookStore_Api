using ShopBook.Data.Mapping_Models;
using ShopBook.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Service
{
    public interface IBookService 
    {
        Task<List<BookMapping>> GetAllBookMapping(string keyWord);
    }
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<List<BookMapping>> GetAllBookMapping(string keyWord)
        {
            return await _bookRepository.GetAllBookMapping(keyWord);
        }
    }
}
