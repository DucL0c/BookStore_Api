using ShopBook.Model.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Data.Dto
{
    public class LoginDto
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
    public class LoginResultDto
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public int ExpiresIn { get; set; }
        public UserLoginDto? User { get; set; }
    }

    public class UserLoginDto
    {
        public int UserId { get; set; }

        public string? Name { get; set; }

        public string Email { get; set; } = null!;

        public string? Role { get; set; }
    }

}
