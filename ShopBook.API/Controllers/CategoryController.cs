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
    public class CategoryController : ControllerBase
    {
        #region Intialize
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }
        #endregion Intialize

        #region Properties
        /// <summary>
        /// lấy danh sách danh mục
        /// </summary>
        /// <returns></returns>
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var model = await _categoryService.GetAll();
                var mapping = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryViewModels>>(model.OrderByDescending(x => x.CategoryId));
                return Ok(mapping);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        /// <summary>
        /// lấy danh sách danh mục theo phân trang
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
                var model = await _categoryService.GetAll(keyword);
                int totalRow = 0;
                var data = model.OrderByDescending(x => x.CategoryId).Skip(page * pageSize).Take(pageSize);
                var mapping = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryViewModels>>(data);

                totalRow = model.Count();

                var paging = new PaginationSet<CategoryViewModels>()
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
        /// lấy danh mục theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var model = await _categoryService.GetById(id);

                // Kiểm tra nếu không có dữ liệu
                if (model == null)
                {
                    return NotFound(new { message = "Không tìm thấy với ID đã cho." });
                }

                var mapping = _mapper.Map<Category, CategoryViewModels>(model);
                return Ok(mapping);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Đã xảy ra lỗi khi truy vấn dữ liệu.", detail = ex.Message });
            }
        }



        /// <summary>
        /// Thêm mới danh mục
        /// </summary>
        /// <param name="seller"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> Create(CategoryViewModels category)
        {
            if (ModelState.IsValid)
            {
                Category us = _mapper.Map<CategoryViewModels, Category>(category);
                try
                {
                    _ = await _categoryService.Add(us);
                    return CreatedAtAction(nameof(Create), new { id = us.CategoryId }, us);
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
        /// chỉnh sửa danh mục
        /// </summary>
        /// <param name="seller"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        public async Task<IActionResult> Update(CategoryViewModels category)
        {
            if (ModelState.IsValid)
            {
                Category mapping = _mapper.Map<CategoryViewModels, Category>(category);
                try
                {
                    _ = await _categoryService.Update(mapping);
                    return CreatedAtAction(nameof(Update), new { id = mapping.CategoryId }, mapping);
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
        /// xóa danh mục theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                Category result = await _categoryService.Delete(id);

                // Kiểm tra nếu không tìm thấy tác giả
                if (result == null)
                {
                    return NotFound(new { message = "Không tìm thấy với ID đã cho." });
                }

                CategoryViewModels responseData = _mapper.Map<Category, CategoryViewModels>(result);
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Đã xảy ra lỗi khi xóa.", detail = ex.Message });
            }
        }



        /// <summary>
        ///  Xóa danh mục theo list Id
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
                            _ = await _categoryService.Delete(item);
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
