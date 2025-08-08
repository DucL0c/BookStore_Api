using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShopBook.API.Infrastructure.Core;
using ShopBook.Data.Dto;
using ShopBook.Data.Models;
using ShopBook.Data.ViewModels;
using ShopBook.Service;

namespace ShopBook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        #region Intilize
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;
        public BookController(IBookService bookService,IMapper mapper)
        {
            _bookService = bookService;
            _mapper = mapper;
        }
        #endregion Intilize

        #region Properties
        /// <summary>
        /// lấy tất cả ko truyền param
        /// </summary>
        /// <returns></returns>
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var books = await _bookService.GetAll();
                return Ok(books); // Trả về danh sách carts dạng JSON
            }
            catch (Exception ex)
            {
                // Ghi log nếu cần: _logger.LogError(ex, "Error occurred while getting carts.");
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi lấy danh sách", error = ex.Message });
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
        public async Task<IActionResult> GetAllByPaging(int page = 0, int pageSize = 100, string? keyword = null)
        {
            try
            {
                // Gọi service để lấy tất cả sách theo keyword (đã mapping sẵn BookDto)
                var model = await _bookService.GetAllByKeyWord(keyword);

                int totalRow = model.Count();

                var data = model
                    .OrderByDescending(x => x.BookId)
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToList();

                var paging = new PaginationSet<BookDto>
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
                return BadRequest(new
                {
                    message = "Đã xảy ra lỗi",
                    detail = ex.Message
                });
            }
        }



        /// <summary>
        /// lấy theo id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("byId/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _bookService.GetBookByIdAsync(id);

            if (result == null)
            {
                return NotFound(new { message = $"Không tìm thấy sách với ID = {id}." });
            }

            return Ok(result);
        }

        /// <summary>
        /// Thêm mới 
        /// </summary>
        /// <param name="seller"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> Create(BookViewModels book)
        {
            if (ModelState.IsValid)
            {
                Book us = _mapper.Map<BookViewModels, Book>(book);
                try
                {
                    _ = await _bookService.Add(us);
                    return CreatedAtAction(nameof(Create), new { id = us.BookId }, us);
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
        public async Task<IActionResult> Update(BookViewModels book)
        {
            if (ModelState.IsValid)
            {
                Book mapping = _mapper.Map<BookViewModels, Book>(book);
                try
                {
                    _ = await _bookService.Update(mapping);
                    return CreatedAtAction(nameof(Update), new { id = mapping.BookId }, mapping);
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
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                Book result = await _bookService.Delete(id);

                // Kiểm tra nếu không tìm thấy tác giả
                if (result == null)
                {
                    return NotFound(new { message = "Không tìm thấy với ID đã cho." });
                }

                BookViewModels responseData = _mapper.Map<Book, BookViewModels>(result);
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
