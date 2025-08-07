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
    public interface IUsersService
    {
        Task<IQueryable<User>> GetAll(string keyword);

        Task<IQueryable<User>> GetAll();

        Task<User> GetById(int id);

        Task<User> Add(User user);

        Task<User> Update(User user);

        Task<User> Delete(int id);

    }
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        public UsersService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<User> Add(User user)
        {
            if (await _usersRepository.CheckContainsAsync(x => x.Email == user.Email))
                throw new NameDuplicatedException("Email đã được sử dụng");
            return await _usersRepository.AddASync(user);
        }

        public async Task<User> Delete(int id)
        {
            return await _usersRepository.DeleteAsync(id);
        }

        public async Task<IQueryable<User>> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return await _usersRepository.GetAllAsync(x => x.FullName.ToUpper().Contains(keyword.ToUpper()) || x.NickName.ToUpper().Contains(keyword.ToUpper()) || x.Email.ToUpper().Contains(keyword.ToUpper()));
            else
                return await _usersRepository.GetAllAsync();
        }

        public async Task<IQueryable<User>> GetAll()
        {
           return await _usersRepository.GetAllAsync();
        }

        public async Task<User> GetById(int id)
        {
            return await _usersRepository.GetByIdAsync(id);
        }

        public async Task<User> Update(User user)
        {
            if (await _usersRepository.CheckContainsAsync(x => x.Email == user.Email && x.UserId != user.UserId))
                throw new NameDuplicatedException("Email đã được sử dụng");
            return await _usersRepository.UpdateASync(user);
        }
    }
}
