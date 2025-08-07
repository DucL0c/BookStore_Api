using ShopBook.Data.Infrastructure;
using ShopBook.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Data.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
       
    }
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        private readonly BookstoreContext _context;
        public CategoryRepository(BookstoreContext context) : base(context)
        {
            _context = context;
        }

    }
}
