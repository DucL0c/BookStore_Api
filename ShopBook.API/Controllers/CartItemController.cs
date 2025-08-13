using AutoMapper;
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
    public class CartItemController : ControllerBase
    {
        #region Intialize
        private readonly ICartItemService _cartItemService;
        private readonly IMapper _mapper;
        public CartItemController(ICartItemService cartItemService, IMapper mapper)
        {
            _cartItemService = cartItemService;
            _mapper = mapper;
        }
        #endregion Intialize

        #region Properties
        /// <summary>
        /// lấy tất cả ko truyền param
        /// </summary>
        /// <returns></returns>
        [HttpGet("getall")]
        [Authorize(Roles = "user,admin")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var cartItems = await _cartItemService.GetAllAsync();
                return Ok(cartItems); // Trả về danh sách cartItems dạng JSON
            }
            catch (Exception ex)
            {
                // Ghi log nếu cần: _logger.LogError(ex, "Error occurred while getting cartItems.");
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi lấy danh sách giỏ hàng.", error = ex.Message });
            }
        }


        /// <summary>
        /// lấy danh sách phân trang
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
                var model = await _cartItemService.GetAllByKeyWord(keyword);
                int totalRow = model.Count();

                var data = model
                    .OrderByDescending(x => x.CartId)
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToList(); 

                var paging = new PaginationSet<CartItem>
                {
                    Items = data,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };

                return Ok(paging);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// lấy theo id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("byId/{Id}")]
        [Authorize(Roles = "user,admin")]
        public async Task<IActionResult> GetById(int Id)
        {
            var result = await _cartItemService.GetById(Id);

            if (result == null || result.Count == 0)
            {
                return NotFound(new { message = "Không tìm thấy bản ghi nào của Id đã cho." });
            }

            return Ok(result);
        }

        /// <summary>
        /// lấy theo userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("byUserId/{userId}")]
        [Authorize(Roles = "user,admin")]

        public async Task<IActionResult> GetByUserAsync(int userId)
        {
            var result = await _cartItemService.GetByUserAsync(userId);
            return Ok(result ?? new List<CartItem>());
        }

        /// <summary>
        /// lấy theo cartId
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        [HttpGet("byCartId/{cartId}")]
        [Authorize(Roles = "user,admin")]

        public async Task<IActionResult> GetByCartId(int cartId)
        {
            var result = await _cartItemService.GetByCartIdAsync(cartId);

            if (result == null || result.Count == 0)
            {
                return NotFound(new { message = "Không tìm thấy bản ghi nào của cartId đã cho." });
            }

            return Ok(result);
        }

        /// <summary>
        /// Thêm mới 
        /// </summary>
        /// <param name="seller"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [Authorize(Roles = "user,admin")]

        public async Task<IActionResult> Create(CartItemViewModels cartItem)
        {
            if (ModelState.IsValid)
            {
                CartItem us = _mapper.Map<CartItemViewModels, CartItem>(cartItem);
                try
                {
                    _ = await _cartItemService.Add(us);
                    return CreatedAtAction(nameof(Create), new { id = us.CartItemId }, us);
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
        /// chỉnh sửa
        /// </summary>
        /// <param name="seller"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        [Authorize(Roles = "user,admin")]

        public async Task<IActionResult> Update(CartItemViewModels cartItem)
        {
            if (ModelState.IsValid)
            {
                CartItem mapping = _mapper.Map<CartItemViewModels, CartItem>(cartItem);
                try
                {
                    _ = await _cartItemService.Update(mapping);
                    return CreatedAtAction(nameof(Update), new { id = mapping.CartItemId }, mapping);
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
        /// xóa danh 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "user,admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                CartItem result = await _cartItemService.Delete(id);

                // Kiểm tra nếu không tìm thấy tác giả
                if (result == null)
                {
                    return NotFound(new { message = "Không tìm thấy với ID đã cho." });
                }

                CartItemViewModels responseData = _mapper.Map<CartItem, CartItemViewModels>(result);
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Đã xảy ra lỗi khi xóa.", detail = ex.Message });
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
                            _ = await _cartItemService.Delete(item);
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
