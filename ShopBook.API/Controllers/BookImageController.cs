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
    public class BookImageController : ControllerBase
    {
        #region Intialize
        private readonly IBookImageService _bookImageService;
        private readonly IMapper _mapper;
        public BookImageController(IBookImageService bookImageService, IMapper mapper)
        {
            _bookImageService = bookImageService;
            _mapper = mapper;
        }
        #endregion Intialize

        #region Properties
        /// <summary>
        /// lấy danh sách ảnh của sách
        /// </summary>
        /// <returns></returns>
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var model = await _bookImageService.GetAll();
                var mapping = _mapper.Map<IEnumerable<BookImage>, IEnumerable<BookImageViewModels>>(model.OrderByDescending(x => x.ImageId));
                return Ok(mapping);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        /// <summary>
        /// Lấy ảnh sách theo Id
        /// </summary>
        /// <param name="id">Id tác giả</param>
        /// <returns>Thông tin tác giả</returns>
        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var model = await _bookImageService.GetById(id);

                // Kiểm tra nếu không có dữ liệu
                if (model == null)
                {
                    return NotFound(new { message = "Không tìm thấy ảnh với ID đã cho." });
                }

                var mapping = _mapper.Map<BookImage, BookImageViewModels>(model);
                return Ok(mapping);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Đã xảy ra lỗi khi truy vấn dữ liệu.", detail = ex.Message });
            }
        }

        /// <summary>
        /// thêm mới hình ảnh sách
        /// </summary>
        /// <param name="bookImage"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> Create(BookImageViewModels bookImage)
        {
            if (ModelState.IsValid)
            {
                BookImage bookImg = _mapper.Map<BookImageViewModels, BookImage>(bookImage);
                try
                {
                    _ = await _bookImageService.Add(bookImg);
                    return CreatedAtAction(nameof(Create), new { id = bookImg.ImageId }, bookImg);
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
        /// <param name="bookImage"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        public async Task<IActionResult> Update(BookImageViewModels bookImage)
        {
            if (ModelState.IsValid)
            {
                BookImage mapping = _mapper.Map<BookImageViewModels, BookImage>(bookImage);
                try
                {
                    _ = await _bookImageService.Update(mapping);
                    return CreatedAtAction(nameof(Update), new { id = mapping.ImageId }, mapping);
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
                BookImage result = await _bookImageService.Delete(id);

                // Kiểm tra nếu không tìm thấy tác giả
                if (result == null)
                {
                    return NotFound(new { message = "Không tìm thấy ảnh với ID đã cho." });
                }

                BookImageViewModels responseData = _mapper.Map<BookImage, BookImageViewModels>(result);
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Đã xảy ra lỗi khi xóa ảnh.", detail = ex.Message });
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
                            _ = await _bookImageService.Delete(item);
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
