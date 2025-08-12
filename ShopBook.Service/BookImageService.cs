using ShopBook.Data.Models;
using ShopBook.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Service
{
    public interface IBookImageService 
    {
        Task<IQueryable<BookImage>> GetAll();
        Task<List<BookImage>> GetAllByKeyWord(string keyword);

        Task<BookImage> GetById(int id);

        Task<BookImage> Add(BookImage image);

        Task<BookImage> Update(BookImage image);

        Task<BookImage> Delete(int id);
    }
    public class BookImageService : IBookImageService
    {
        private readonly IBookImageRepository _bookImageRepository;
        public BookImageService(IBookImageRepository bookImageRepository)
        {
            _bookImageRepository = bookImageRepository;
        }
        public async Task<BookImage> Add(BookImage image)
        {
            return await _bookImageRepository.AddASync(image);
        }

        public async Task<BookImage> Delete(int id)
        {
            return await _bookImageRepository.DeleteAsync(id);  
        }

        public async Task<IQueryable<BookImage>> GetAll()
        {
            return await _bookImageRepository.GetAllAsync();
        }

        public async Task<List<BookImage>> GetAllByKeyWord(string keyword)
        {
            return await _bookImageRepository.GetAllByKeyWord(keyword);
        }

        public async Task<BookImage> GetById(int id)
        {
            return await _bookImageRepository.GetByIdAsync(id);
        }

        public async Task<BookImage> Update(BookImage image)
        {
            return await _bookImageRepository.UpdateASync(image);
        }
    }
}
