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
        public string? Token { get; set; }
        public int ExpiresIn { get; set; }
        public UserDto? User { get; set; }
    }
}
