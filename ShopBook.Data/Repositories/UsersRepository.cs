using Microsoft.EntityFrameworkCore;
using ShopBook.Data.Infrastructure;
using ShopBook.Data.Models;
using ShopBook.Model.Dtos;
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
        Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
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

        public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
        {
            return await _dbContext.Users
                .Where(u => u.RefreshToken == refreshToken)
                .Select(u => new User
                {
                    UserId = u.UserId,
                    Name = u.Name,
                    Email = u.Email,
                    Password = u.Password,
                    RefreshToken = u.RefreshToken,
                    ExpiryDate = u.ExpiryDate,
                    Role = u.Role,
                    Phone = u.Phone,
                    Address = u.Address,
                    Gender = u.Gender,
                    BirthDay = u.BirthDay
                })
                .FirstOrDefaultAsync();
        }
    }
 
 }
