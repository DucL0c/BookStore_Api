using ShopBook.Common.Exceptions;
using ShopBook.Data.Models;
using ShopBook.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Service
{
    public interface IAuthorService
    {
        Task<IQueryable<Author>> GetAll(string keyword);

        Task<IQueryable<Author>> GetAll();

        Task<Author> GetById(int id);

        Task<Author> Add(Author author);

        Task<Author> Update(Author author);

        Task<Author> Delete(int id);
    }
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<Author> Add(Author author)
        {
            return await _authorRepository.AddASync(author);
        }

        public async Task<Author> Delete(int id)
        {
            return await _authorRepository.DeleteAsync(id);
        }

        public async Task<IQueryable<Author>> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return await _authorRepository.GetAllAsync(x => x.Name.ToUpper().Contains(keyword.ToUpper()) || x.Slug.ToUpper().Contains(keyword.ToUpper()));
            else
                return await _authorRepository.GetAllAsync();
        }

        public async Task<IQueryable<Author>> GetAll()
        {
            return await _authorRepository.GetAllAsync();
        }

        public async Task<Author> GetById(int id)
        {
            return await _authorRepository.GetByIdAsync(id);
        }

        public async Task<Author> Update(Author author)
        {
            return await _authorRepository.UpdateASync(author);
        }
    }
}
