using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopBook.API.Infrastructure.Core;
using ShopBook.Data.Models;
using ShopBook.Data.ViewModels;
using ShopBook.Service;

namespace ShopBook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {

        #region Intialize
        private readonly IAuthorService _authorService;
        private readonly IMapper _mapper;
        public AuthorController(IAuthorService authorService, IMapper mapper)
        {
            _authorService = authorService;
            _mapper = mapper;
        }
        #endregion Intialize

        #region Properties
        /// <summary>
        /// lấy danh sách tác giả
        /// </summary>
        /// <returns></returns>
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var model = await _authorService.GetAll();
                var mapping = _mapper.Map<IEnumerable<Author>, IEnumerable<AuthorViewModels>>(model.OrderByDescending(x => x.AuthorId));
                return Ok(mapping);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        /// <summary>
        /// lấy danh sách tác giả phân trang
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
                var model = await _authorService.GetAll(keyword);
                int totalRow = 0;
                var data = model.OrderByDescending(x => x.AuthorId).Skip(page * pageSize).Take(pageSize);
                var mapping = _mapper.Map<IEnumerable<Author>, IEnumerable<AuthorViewModels>>(data);

                totalRow = model.Count();

                var paging = new PaginationSet<AuthorViewModels>()
                {
                    Items = mapping,
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
        /// Lấy tác giả theo Id
        /// </summary>
        /// <param name="id">Id tác giả</param>
        /// <returns>Thông tin tác giả</returns>
        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var model = await _authorService.GetById(id);

                // Kiểm tra nếu không có dữ liệu
                if (model == null)
                {
                    return NotFound(new { message = "Không tìm thấy tác giả với ID đã cho." });
                }

                var mapping = _mapper.Map<Author, AuthorViewModels>(model);
                return Ok(mapping);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Đã xảy ra lỗi khi truy vấn dữ liệu.", detail = ex.Message });
            }
        }



        /// <summary>
        /// thêm mới tác giả
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> Create(AuthorViewModels author)
        {
            if (ModelState.IsValid)
            {
                Author us = _mapper.Map<AuthorViewModels, Author>(author);
                try
                {
                    _ = await _authorService.Add(us);
                    return CreatedAtAction(nameof(Create), new { id = us.AuthorId }, us);
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
        /// Chỉnh sửa tác giả
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        public async Task<IActionResult> Update(AuthorViewModels user)
        {
            if (ModelState.IsValid)
            {
                Author mapping = _mapper.Map<AuthorViewModels, Author>(user);
                try
                {
                    _ = await _authorService.Update(mapping);
                    return CreatedAtAction(nameof(Update), new { id = mapping.AuthorId }, mapping);
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
                Author result = await _authorService.Delete(id);

                // Kiểm tra nếu không tìm thấy tác giả
                if (result == null)
                {
                    return NotFound(new { message = "Không tìm thấy tác giả với ID đã cho." });
                }

                AuthorViewModels responseData = _mapper.Map<Author, AuthorViewModels>(result);
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Đã xảy ra lỗi khi xóa tác giả.", detail = ex.Message });
            }
        }

        #endregion Properties
    }
}
