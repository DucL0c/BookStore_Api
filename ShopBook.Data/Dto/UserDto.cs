using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Model.Dtos
{
    public class UserDto
    {
        public int UserId { get; set; }

        public string? Name { get; set; }

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string? RefreshToken { get; set; }
        public DateTime? ExpiryDate { get; set; }

        public string? Role { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public string? Gender { get; set; }

        public DateOnly? BirthDay { get; set; }
    }
}
