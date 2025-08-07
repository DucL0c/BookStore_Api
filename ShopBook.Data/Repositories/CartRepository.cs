using ShopBook.Data.Infrastructure;
using ShopBook.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Data.Repositories
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<List<Cart>> GetByUserIdAsync(int userId);
        Task<List<Cart>> GetAllAsync(string keyWord);
    }
    internal class CartRepository
    {
    }
}
