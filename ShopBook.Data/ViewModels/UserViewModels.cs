using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Data.ViewModels
{
    public class UserViewModels
    {
        public int UserId { get; set; }

        public string? FullName { get; set; }

        public string? NickName { get; set; }

        public string Email { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public string? Role { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public string? Gender { get; set; }

        public DateOnly? BirthDay { get; set; }

        public DateTime? CreatedAt { get; set; }


    }
}
