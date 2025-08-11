using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShopBook.Data.Models;
using ShopBook.Data.ViewModels;
using ShopBook.Service;

namespace ShopBook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductReviewController : ControllerBase
    {
        #region Intialize
        private readonly IProductReviewService _productReviewService;
        private readonly IMapper _mapper;
        public ProductReviewController(IProductReviewService productReviewService, IMapper mapper)
        {
            _productReviewService = productReviewService;
            _mapper = mapper;
        }
        #endregion Intialize

        #region Properties
        /// <summary>
        /// lấy danh sách 
        /// </summary>
        /// <returns></returns>
        [HttpGet("getall")]
        [Authorize(Roles = "user")]

        public async Task<IActionResult> GetAll()
        {
            var result = await _productReviewService.GetAllAsync();

            if (result == null || result.Count == 0)
                return NotFound(new { message = "Không có bản ghi nào." });

            return Ok(result);
        }

        /// <summary>
        /// lấy theo id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("byId/{Id}")]
        [Authorize(Roles = "user")]

        public async Task<IActionResult> GetById(int Id)
        {
            var result = await _productReviewService.GetById(Id);

            if (result == null || result.Count == 0)
            {
                return NotFound(new { message = "Không tìm thấy bản ghi nào của Id đã cho." });
            }

            return Ok(result);
        }

        /// <summary>
        /// lấy theo bookId
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        [HttpGet("bybook/{bookId}")]
        [Authorize(Roles = "user")]

        public async Task<IActionResult> GetByBookId(int bookId)
        {
            var result = await _productReviewService.GetByBookIdAsync(bookId);

            if (result == null || result.Count == 0)
            {
                return NotFound(new { message = "Không tìm thấy bản ghi nào cho BookId đã cho." });
            }

            return Ok(result);
        }

        /// <summary>
        /// Lấy theo userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("byseller/{userId}")]
        [Authorize(Roles = "user")]

        public async Task<IActionResult> GetBySellerId(int userId)
        {
            var result = await _productReviewService.GetByUserIdAsync(userId);

            if (result == null || result.Count == 0)
            {
                return NotFound(new { message = "Không tìm thấy bản ghi nào đã cho." });
            }

            return Ok(result);
        }


        /// <summary>
        /// thêm mới 
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [Authorize(Roles = "user")]

        public async Task<IActionResult> Create(ProductReviewViewModels productReview)
        {
            if (ModelState.IsValid)
            {
                ProductReview us = _mapper.Map<ProductReviewViewModels, ProductReview>(productReview);
                us.ReviewDate = DateTime.Now;
                try
                {
                    _ = await _productReviewService.Add(us);
                    return CreatedAtAction(nameof(Create), new { id = us.ReviewId }, us);
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
        /// Chỉnh sửa người bán sách
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        [Authorize(Roles = "user")]

        public async Task<IActionResult> Update(ProductReviewViewModels productReview)
        {
            if (ModelState.IsValid)
            {
                ProductReview mapping = _mapper.Map<ProductReviewViewModels, ProductReview>(productReview);
                try
                {
                    _ = await _productReviewService.Update(mapping);
                    return CreatedAtAction(nameof(Update), new { id = mapping.ReviewId }, mapping);
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
        /// Xóa tác giả theo Id
        /// </summary>
        /// <param name="id">ID của tác giả</param>
        /// <returns>Kết quả xóa tác giả</returns>
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "user")]

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                ProductReview result = await _productReviewService.Delete(id);

                // Kiểm tra nếu không tìm thấy tác giả
                if (result == null)
                {
                    return NotFound(new { message = "Không tìm thấy với ID đã cho." });
                }

                ProductReviewViewModels responseData = _mapper.Map<ProductReview, ProductReviewViewModels>(result);
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Đã xảy ra lỗi khi xóa", detail = ex.Message });
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
                            _ = await _productReviewService.Delete(item);
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
