using ShopBook.Data.Models;
using ShopBook.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Service
{
    public interface ISellerService
    {
        Task<IQueryable<Seller>> GetAll(string keyword);

        Task<IQueryable<Seller>> GetAll();

        Task<Seller> GetById(int id);

        Task<Seller> Add(Seller seller);

        Task<Seller> Update(Seller seller);

        Task<Seller> Delete(int id);
    }
    public class SellerService : ISellerService
    {
        private readonly ISellerRepository _sellerRepository;
        public SellerService(ISellerRepository sellerRepository)
        {
            _sellerRepository = sellerRepository;
        }

        public async Task<Seller> Add(Seller seller)
        {
            return await _sellerRepository.AddASync(seller);
        }

        public async Task<Seller> Delete(int id)
        {
            return await _sellerRepository.DeleteAsync(id);
        }

        public async Task<IQueryable<Seller>> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return await _sellerRepository.GetAllAsync(x => x.Name.ToUpper().Contains(keyword.ToUpper()));
            else
                return await _sellerRepository.GetAllAsync();
        }

        public async Task<IQueryable<Seller>> GetAll()
        {
           return await _sellerRepository.GetAllAsync();
        }

        public async Task<Seller> GetById(int id)
        {
            return await _sellerRepository.GetByIdAsync(id);
        }

        public async Task<Seller> Update(Seller seller)
        {
           return await _sellerRepository.UpdateASync(seller);
        }
    }
}
