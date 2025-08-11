using AutoMapper;
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
    public class BookSpecificationController : ControllerBase
    {
        #region Intialize
        private readonly IBookSpecificationService _bookSpecificationService;
        private readonly IMapper _mapper;
        public BookSpecificationController(IBookSpecificationService bookSpecificationService, IMapper mapper)
        {
            _bookSpecificationService = bookSpecificationService;
            _mapper = mapper;
        }
        #endregion Intialize

        #region Properties
        /// <summary>
        /// lấy danh sách 
        /// </summary>
        /// <returns></returns>
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _bookSpecificationService.GetAllAsync();

            if (result == null || result.Count == 0)
                return NotFound(new { message = "Không có bản ghi nào." });

            return Ok(result);
        }

        /// <summary>
        /// lấy theo Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("byId/{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var result = await _bookSpecificationService.GetById(Id);

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
            var result = await _bookSpecificationService.GetByBookIdAsync(bookId);

            if (result == null || result.Count == 0)
            {
                return NotFound(new { message = "Không tìm thấy bản ghi nào cho BookId đã cho." });
            }

            return Ok(result);
        }

        /// <summary>
        /// thêm mới
        /// </summary>
        /// <param name="bookSpecification"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> Create(BookSpecificationViewModels bookSpecification)
        {
            if (ModelState.IsValid)
            {
                BookSpecification us = _mapper.Map<BookSpecificationViewModels, BookSpecification>(bookSpecification);
                try
                {
                    _ = await _bookSpecificationService.Add(us);
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
        public async Task<IActionResult> Update(BookSpecificationViewModels bookSpecification)
        {
            if (ModelState.IsValid)
            {
                BookSpecification mapping = _mapper.Map<BookSpecificationViewModels, BookSpecification>(bookSpecification);
                try
                {
                    _ = await _bookSpecificationService.Update(mapping);
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
                BookSpecification result = await _bookSpecificationService.Delete(id);

                // Kiểm tra nếu không tìm thấy tác giả
                if (result == null)
                {
                    return NotFound(new { message = "Không tìm thấy với ID đã cho." });
                }

                BookSpecificationViewModels responseData = _mapper.Map<BookSpecification, BookSpecificationViewModels>(result);
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
                            _ = await _bookSpecificationService.Delete(item);
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
