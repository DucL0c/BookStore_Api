using ShopBook.Data.Infrastructure;
using ShopBook.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Data.Repositories
{
    public interface ISellerRepository : IRepository<Seller>
    {
        
    }
    public class SellerRepository : RepositoryBase<Seller>, ISellerRepository
    {
        private readonly BookstoreContext _context;
        public SellerRepository(BookstoreContext context) : base(context)
        {
            _context = context;
        }
    }
}
