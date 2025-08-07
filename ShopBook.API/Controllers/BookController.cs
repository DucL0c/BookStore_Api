using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopBook.API.Infrastructure.Core;
using ShopBook.Data.Mapping_Models;
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

        /// <summary>
        /// Lấy danh sách book phân trang
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet("getlistpaging")]
        public async Task<IActionResult> GetListPaging(int page = 0, int pageSize = 10, string? keyword = null)
        {
            try
            {
                var allBooks = await _bookService.GetAllBookMapping(keyword); // Trả về List<BookMapping>
                var totalRow = allBooks.Count;

                var pagedBooks = allBooks
                    .OrderByDescending(x => x.BookId)
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToList();

                var responseData = new PaginationSet<BookMapping>
                {
                    Items = pagedBooks,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };

                return Ok(responseData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
