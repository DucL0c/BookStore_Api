using ShopBook.Data.Infrastructure;
using ShopBook.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Data.Repositories
{
    public interface IUsersRepository : IRepository<User>
    {
        
    }
    public class UsersRepository : RepositoryBase<User>, IUsersRepository
    {
        private BookstoreContext _dbContext;
        public UsersRepository(BookstoreContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
 
 }
