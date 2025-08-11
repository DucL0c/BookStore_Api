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
    public class OrderController : ControllerBase
    {
        #region Intialize
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        public OrderController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
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
                var orders = await _orderService.GetAllAsync();
                return Ok(orders); // Trả về danh sách carts dạng JSON
            }
            catch (Exception ex)
            {
                // Ghi log nếu cần: _logger.LogError(ex, "Error occurred while getting carts.");
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
                var model = await _orderService.GetAllAsyncByKeyWord(keyword);
                int totalRow = model.Count();

                var data = model
                    .OrderByDescending(x => x.OrderId)
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToList(); // Trả thẳng Order

                var paging = new PaginationSet<Order>
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
            var result = await _orderService.GetById(Id);

            if (result == null || result.Count == 0)
            {
                return NotFound(new { message = "Không tìm thấy bản ghi nào của Id đã cho." });
            }

            return Ok(result);
        }

        /// <summary>
        /// lấy theo UserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("byUserId/{userId}")]
        [Authorize(Roles = "user,admin")]

        public async Task<IActionResult> GetByUserId(int userId)
        {
            var result = await _orderService.GetByUserIdAsync(userId);

            if (result == null || result.Count == 0)
            {
                return NotFound(new { message = "Không tìm thấy bản ghi nào của userId đã cho." });
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

        public async Task<IActionResult> Create(OrderViewModels order)
        {
            if (ModelState.IsValid)
            {
                Order us = _mapper.Map<OrderViewModels, Order>(order);
                try
                {
                    _ = await _orderService.Add(us);
                    return CreatedAtAction(nameof(Create), new { id = us.OrderId }, us);
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

        public async Task<IActionResult> Update(OrderViewModels order)
        {
            if (ModelState.IsValid)
            {
                Order mapping = _mapper.Map<OrderViewModels, Order>(order);
                try
                {
                    _ = await _orderService.Update(mapping);
                    return CreatedAtAction(nameof(Update), new { id = mapping.OrderId }, mapping);
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
                Order result = await _orderService.Delete(id);

                // Kiểm tra nếu không tìm thấy tác giả
                if (result == null)
                {
                    return NotFound(new { message = "Không tìm thấy với ID đã cho." });
                }

                OrderViewModels responseData = _mapper.Map<Order, OrderViewModels>(result);
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
                            _ = await _orderService.Delete(item);
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
