using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopBook.Data.Models;
using ShopBook.Data.ViewModels;
using ShopBook.Service;

namespace ShopBook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookAuthorController : ControllerBase
    {
        #region Intialize
        private readonly IBookAuthorService _bookAuthorService;
        private readonly IMapper _mapper;
        public BookAuthorController(IBookAuthorService bookAuthorService, IMapper mapper)
        {
            _bookAuthorService = bookAuthorService;
            _mapper = mapper;
        }
        #endregion Intialize

        #region Properties
        /// <summary>
        /// lấy danh sách tác giả sách
        /// </summary>
        /// <returns></returns>
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _bookAuthorService.GetAllAsync();

            if (result == null || result.Count == 0)
                return NotFound(new { message = "Không có bản ghi bookAuthor nào." });

            return Ok(result);
        }

        /// <summary>
        /// lấy theo bookId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("byId/{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var result = await _bookAuthorService.GetById(Id);

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
            var result = await _bookAuthorService.GetByBookIdAsync(bookId);

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
            var result = await _bookAuthorService.GetBySellerIdAsync(sellerId);

            if (result == null || result.Count == 0)
            {
                return NotFound(new { message = "Không tìm thấy bản ghi nào cho SellerId đã cho." });
            }

            return Ok(result);
        }


        /// <summary>
        /// thêm mới 
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> Create(BookAuthorViewModels bookAuthor)
        {
            if (ModelState.IsValid)
            {
                BookAuthor us = _mapper.Map<BookAuthorViewModels, BookAuthor>(bookAuthor);
                try
                {
                    _ = await _bookAuthorService.Add(us);
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
        /// Chỉnh sửa 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        public async Task<IActionResult> Update(BookAuthorViewModels bookSeller)
        {
            if (ModelState.IsValid)
            {
                BookAuthor mapping = _mapper.Map<BookAuthorViewModels, BookAuthor>(bookSeller);
                try
                {
                    _ = await _bookAuthorService.Update(mapping);
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
                BookAuthor result = await _bookAuthorService.Delete(id);

                // Kiểm tra nếu không tìm thấy tác giả
                if (result == null)
                {
                    return NotFound(new { message = "Không tìm thấy với ID đã cho." });
                }

                BookAuthorViewModels responseData = _mapper.Map<BookAuthor, BookAuthorViewModels>(result);
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Đã xảy ra lỗi khi xóa.", detail = ex.Message });
            }
        }


        #endregion Properties
    }
}
