using Microsoft.EntityFrameworkCore;
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
        Task<User?> GetUserByEmailAsync(string email);
    }
    public class UsersRepository : RepositoryBase<User>, IUsersRepository
    {
        private BookstoreContext _dbContext;
        public UsersRepository(BookstoreContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _dbContext.Set<User>().FirstOrDefaultAsync(u => u.Email == email);
        }
    }
 
 }
