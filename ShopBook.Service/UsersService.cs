using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ShopBook.Common.Exceptions;
using ShopBook.Data.Dto;
using ShopBook.Data.Models;
using ShopBook.Data.Repositories;
using ShopBook.Model.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

        Task<string> RegisterAsync(RegisterDto dto);
        Task<LoginResultDto> LoginAsync(LoginDto dto);

    }
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        public UsersService(IUsersRepository usersRepository , IConfiguration configuration,IMapper mapper)
        {
            _usersRepository = usersRepository;
            _config = configuration;
            _mapper = mapper;
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

        public async Task<LoginResultDto> LoginAsync(LoginDto dto)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            var user = await _usersRepository.GetUserByEmailAsync(dto.Email);
#pragma warning restore CS8604 // Possible null reference argument.
            if (user == null)
            {
                throw new Exception("User not found");
            }
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                throw new Exception("Invalid password");
            }
            var result = new LoginResultDto
            {
                Token = GenerateJwt(user),
                User = _mapper.Map<UserDto>(user),
                ExpiresIn = int.Parse(_config["Jwt:ExpireDays"] ?? "7") * 24 * 60 * 60
            };
            return result;
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            var user = await _usersRepository.GetUserByEmailAsync(dto.Email);
#pragma warning restore CS8604 // Possible null reference argument.
            if (user != null)
            {
                throw new Exception("Email already exists");
            }

            var newUser = _mapper.Map<User>(dto);
            newUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash);
            newUser.Role = "user";

            await _usersRepository.AddASync(newUser);
            return GenerateJwt(newUser);
        }

        public async Task<User> Update(User user)
        {
            if (await _usersRepository.CheckContainsAsync(x => x.Email == user.Email && x.UserId != user.UserId))
                throw new NameDuplicatedException("Email đã được sử dụng");
            return await _usersRepository.UpdateASync(user);
        }

        private string GenerateJwt(User user)
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role ?? "user")
            };
            #pragma warning restore CS8604 // Possible null reference argument.

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
            );
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(
                    int.Parse(_config["Jwt:ExpireDays"] ?? "7")
                ),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
