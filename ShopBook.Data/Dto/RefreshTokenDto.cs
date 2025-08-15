using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Data.Dto
{
    public class RefreshTokenRequestDto
    {
        public string? RefreshToken { get; set; }
    }
    public class RefreshTokenResponseDto
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
