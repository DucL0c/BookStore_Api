using ShopBook.Data.Infrastructure;
using ShopBook.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Data.Repositories
{
    public interface IAuthorRepository : IRepository<Author>
    {
        
    }
        
    public class AuthorRepository : RepositoryBase<Author>, IAuthorRepository
    {
        private readonly BookstoreContext _context;
        public AuthorRepository(BookstoreContext context) : base(context)
        {
            _context = context;
        }
    }
}
