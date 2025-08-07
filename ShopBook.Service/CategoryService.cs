using ShopBook.Data.Models;
using ShopBook.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Service
{
    public interface ICategoryService
    {
        Task<IQueryable<Category>> GetAll(string keyword);

        Task<IQueryable<Category>> GetAll();

        Task<Category> GetById(int id);

        Task<Category> Add(Category category);

        Task<Category> Update(Category category);

        Task<Category> Delete(int id);
    }
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<Category> Add(Category category)
        {
            return await _categoryRepository.AddASync(category);
        }
        public async Task<Category> Delete(int id)
        {
            return await _categoryRepository.DeleteAsync(id);
        }
        public async Task<IQueryable<Category>> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return await _categoryRepository.GetAllAsync(x => x.Name.ToUpper().Contains(keyword.ToUpper()));
            else
                return await _categoryRepository.GetAllAsync();
        }
        public async Task<IQueryable<Category>> GetAll()
        {
            return await _categoryRepository.GetAllAsync();
        }
        public async Task<Category> GetById(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }
        public async Task<Category> Update(Category category)
        {
            return await _categoryRepository.UpdateASync(category);
        }
    }
}
