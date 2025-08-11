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
    public class OrderItemController : ControllerBase
    {
        #region Intialize
        private readonly IOrderItemService _orderItemService;
        private readonly IMapper _mapper;
        public OrderItemController(IOrderItemService orderItemService, IMapper mapper)
        {
            _orderItemService = orderItemService;
            _mapper = mapper;
        }
        #endregion Intialize

        #region Properties
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
                var model = await _orderItemService.GetAllAsync(keyword);
                int totalRow = model.Count();

                var data = model
                    .OrderByDescending(x => x.OrderId)
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToList(); // Trả thẳng OrderItem

                var paging = new PaginationSet<OrderItem>
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
            var result = await _orderItemService.GetById(Id);

            if (result == null || result.Count == 0)
            {
                return NotFound(new { message = "Không tìm thấy bản ghi nào của Id đã cho." });
            }

            return Ok(result);
        }

        /// <summary>
        /// lấy theo BookId
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        [HttpGet("byBookId/{bookId}")]
        [Authorize(Roles = "user,admin")]

        public async Task<IActionResult> GetByBookId(int bookId)
        {
            var result = await _orderItemService.GetByBookIdAsync(bookId);

            if (result == null || result.Count == 0)
            {
                return NotFound(new { message = "Không tìm thấy bản ghi nào của BookId đã cho." });
            }

            return Ok(result);
        }

        /// <summary>
        /// lấy theo OrderId
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("byOrderId/{orderId}")]
        [Authorize(Roles = "user,admin")]

        public async Task<IActionResult> GetByOrderId(int orderId)
        {
            var result = await _orderItemService.GetByOrderIdAsync(orderId);

            if (result == null || result.Count == 0)
            {
                return NotFound(new { message = "Không tìm thấy bản ghi nào của orderId đã cho." });
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

        public async Task<IActionResult> Create(OrderItemViewModels orderItem)
        {
            if (ModelState.IsValid)
            {
                OrderItem us = _mapper.Map<OrderItemViewModels, OrderItem>(orderItem);
                try
                {
                    _ = await _orderItemService.Add(us);
                    return CreatedAtAction(nameof(Create), new { id = us.OrderItemId }, us);
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

        public async Task<IActionResult> Update(OrderItemViewModels orderItem)
        {
            if (ModelState.IsValid)
            {
                OrderItem mapping = _mapper.Map<OrderItemViewModels, OrderItem>(orderItem);
                try
                {
                    _ = await _orderItemService.Update(mapping);
                    return CreatedAtAction(nameof(Update), new { id = mapping.OrderItemId }, mapping);
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
                OrderItem result = await _orderItemService.Delete(id);

                // Kiểm tra nếu không tìm thấy tác giả
                if (result == null)
                {
                    return NotFound(new { message = "Không tìm thấy với ID đã cho." });
                }

                OrderItemViewModels responseData = _mapper.Map<OrderItem, OrderItemViewModels>(result);
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
                            _ = await _orderItemService.Delete(item);
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
