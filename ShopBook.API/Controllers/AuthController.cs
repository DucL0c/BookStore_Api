using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopBook.Data.Dto;
using ShopBook.Service;

namespace ShopBook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region Initialize
        private readonly IUsersService _usersService;
        public AuthController(IUsersService usersService)
        {
            _usersService = usersService;
        }
        #endregion

        #region properties

        /// <summary>
        /// Đăng ký tài khoản mới
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid registration data.");
            }
            try
            {
                var token = await _usersService.RegisterAsync(dto);
                return Ok("Register success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Đăng nhập
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid login data.");
            }
            try
            {
                var result = await _usersService.LoginAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
    #endregion properties
}
