using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopBook.Data.Dto;
using ShopBook.Service;
using System.Security.Claims;

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

        /// <summary>
        /// RefreshToken
        /// </summary>
        [HttpPost("refreshtoken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
                return BadRequest("Refresh token is required");

            try
            {
                var result = await _usersService.RefreshTokenAsync(request.RefreshToken);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Logout
        /// </summary>
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            if (userId == 0)
                return Unauthorized();

            await _usersService.LogoutAsync(userId);

            return Ok(new { message = "Logout successful" });
        }
    }
    #endregion properties
}
