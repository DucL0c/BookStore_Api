using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShopBook.API.Infrastructure.Core;
using ShopBook.Data.Models;
using ShopBook.Data.ViewModels;
using ShopBook.Service;

namespace ShopBook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        #region Intialize
        private readonly IUsersService _usersService;
        private readonly IMapper _mapper;
        public UsersController(IUsersService usersService, IMapper mapper)
        {
            _usersService = usersService;
            _mapper = mapper;
        }
        #endregion Intialize

        #region Properties
        /// <summary>
        /// Lấy danh sách người dùng
        /// </summary>
        /// <returns></returns>
        [HttpGet("getall")]
        [Authorize(Roles = "user,admin")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var model = await _usersService.GetAll();
                var mapping = _mapper.Map<IEnumerable<User>, IEnumerable<UserViewModels>>(model.OrderByDescending(x => x.UserId));
                return Ok(mapping);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        /// <summary>
        /// lấy danh sách người dùng phân trang
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet("getallbypaging")]
        [Authorize(Roles = "user,admin")]
        public async Task<IActionResult> GetAllByPaging(int page = 0, int pageSize = 100, string? keyword = null)
        {
            try
            {
                var model = await _usersService.GetAll(keyword);
                int totalRow = 0;
                var data = model.OrderByDescending(x => x.UserId).Skip(page * pageSize).Take(pageSize);
                var mapping = _mapper.Map<IEnumerable<User>, IEnumerable<UserViewModels>>(data);

                totalRow = model.Count();

                var paging = new PaginationSet<UserViewModels>()
                {
                    Data = mapping,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };
                return Ok(paging);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get thông tin người dùng theo Id
        /// </summary>
        /// <param name="id">Id người dùng</param>
        /// <returns>Thông tin người dùng hoặc lỗi</returns>
        [HttpGet("getbyid/{id}")]
        [Authorize(Roles = "user,admin")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var model = await _usersService.GetById(id);

                // Kiểm tra nếu không tìm thấy
                if (model == null)
                {
                    return NotFound(new { message = "Không tìm thấy người dùng với ID đã cho." });
                }

                var mapping = _mapper.Map<User, UserViewModels>(model);
                return Ok(mapping);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Đã xảy ra lỗi khi truy vấn người dùng.", detail = ex.Message });
            }
        }



        /// <summary>
        /// thêm mới người dùng
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [Authorize(Roles = "user,admin")]
        public async Task<IActionResult> Create(UserViewModels user)
        {
            if (ModelState.IsValid)
            {
                User us = _mapper.Map<UserViewModels, User>(user);
                us.CreatedAt = DateTime.Now;
                us.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
                try
                {
                    _ = await _usersService.Add(us);
                    return CreatedAtAction(nameof(Create), new { id = us.UserId }, us);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Chỉnh sửa người dùng
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        [Authorize(Roles = "user,admin")]
        public async Task<IActionResult> Update(UserViewModels user)
        {
            if (ModelState.IsValid)
            {
                User mapping = _mapper.Map<UserViewModels, User>(user);
                mapping.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
                try
                {
                    _ = await _usersService.Update(mapping);
                    return CreatedAtAction(nameof(Update), new { id = mapping.UserId }, mapping);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Xóa người dùng theo Id
        /// </summary>
        /// <param name="id">ID người dùng</param>
        /// <returns>Kết quả xóa</returns>
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "user,admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                User result = await _usersService.Delete(id);

                // Kiểm tra nếu không tìm thấy người dùng
                if (result == null)
                {
                    return NotFound(new { message = "Không tìm thấy người dùng với ID đã cho." });
                }

                UserViewModels responseData = _mapper.Map<User, UserViewModels>(result);
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Đã xảy ra lỗi khi xóa người dùng.", detail = ex.Message });
            }
        }

        /// <summary>
        /// xóa nhiều 
        /// </summary>
        /// <param name="checkedList"></param>
        /// <returns></returns>
        [HttpDelete("deletemulti")]
        public async Task<IActionResult> DeleteMulti(string checkedList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                try
                {
                    int countSuccess = 0;
                    int countError = 0;
                    List<int> result = new();
                    List<int>? listItem = JsonConvert.DeserializeObject<List<int>>(checkedList);
                    foreach (int item in listItem)
                    {
                        try
                        {
                            _ = await _usersService.Delete(item);
                            countSuccess++;
                        }
                        catch (Exception)
                        {
                            countError++;
                        }
                    }
                    result.Add(countSuccess);
                    result.Add(countError);

                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        #endregion Properties

    }
}
