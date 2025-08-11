using AutoMapper;
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
    public class BookSellerController : ControllerBase
    {
        #region Intialize
        private readonly IBookSellerService _bookSellerService;
        private readonly IMapper _mapper;
        public BookSellerController(IBookSellerService bookSellerService, IMapper mapper)
        {
            _bookSellerService = bookSellerService;
            _mapper = mapper;
        }
        #endregion Intialize

        #region Properties
        /// <summary>
        /// lấy danh sách người bán sách
        /// </summary>
        /// <returns></returns>
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _bookSellerService.GetAllAsync();

            if (result == null || result.Count == 0)
                return NotFound(new { message = "Không có bản ghi BookSeller nào." });

            return Ok(result);
        }

        /// <summary>
        /// lấy theo id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("byId/{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var result = await _bookSellerService.GetById(Id);

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
        public async Task<IActionResult> GetByBookId(int bookId)
        {
            var result = await _bookSellerService.GetByBookIdAsync(bookId);

            if (result == null || result.Count == 0)
            {
                return NotFound(new { message = "Không tìm thấy bản ghi nào cho BookId đã cho." });
            }

            return Ok(result);
        }

        /// <summary>
        /// Lấy theo sellerId
        /// </summary>
        /// <param name="sellerId"></param>
        /// <returns></returns>
        [HttpGet("byseller/{sellerId}")]
        public async Task<IActionResult> GetBySellerId(int sellerId)
        {
            var result = await _bookSellerService.GetBySellerIdAsync(sellerId);

            if (result == null || result.Count == 0)
            {
                return NotFound(new { message = "Không tìm thấy bản ghi nào cho SellerId đã cho." });
            }

            return Ok(result);
        }


        /// <summary>
        /// thêm mới người bán sách
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> Create(BookSellerViewModels bookSeller)
        {
            if (ModelState.IsValid)
            {
                BookSeller us = _mapper.Map<BookSellerViewModels, BookSeller>(bookSeller);
                try
                {
                    _ = await _bookSellerService.Add(us);
                    return CreatedAtAction(nameof(Create), new { id = us.Id }, us);
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
        public async Task<IActionResult> Update(BookSellerViewModels bookSeller)
        {
            if (ModelState.IsValid)
            {
                BookSeller mapping = _mapper.Map<BookSellerViewModels, BookSeller>(bookSeller);
                try
                {
                    _ = await _bookSellerService.Update(mapping);
                    return CreatedAtAction(nameof(Update), new { id = mapping.Id }, mapping);
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
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                BookSeller result = await _bookSellerService.Delete(id);

                // Kiểm tra nếu không tìm thấy tác giả
                if (result == null)
                {
                    return NotFound(new { message = "Không tìm thấy với ID đã cho." });
                }

                BookSellerViewModels responseData = _mapper.Map<BookSeller, BookSellerViewModels>(result);
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
                            _ = await _bookSellerService.Delete(item);
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
