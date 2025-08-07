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
    public class SellerController : ControllerBase
    {
        #region Intialize
        private readonly ISellerService _sellerService;
        private readonly IMapper _mapper;
        public SellerController(ISellerService sellerService, IMapper mapper)
        {
            _sellerService = sellerService;
            _mapper = mapper;
        }
        #endregion Intialize

        #region Properties
        /// <summary>
        /// lấy danh sách seller no param
        /// </summary>
        /// <returns></returns>
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var model = await _sellerService.GetAll();
                var mapping = _mapper.Map<IEnumerable<Seller>, IEnumerable<SellerViewModels>>(model.OrderByDescending(x => x.SellerId));
                return Ok(mapping);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        /// <summary>
        /// lấy danh sách seller theo phân trang 
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
                var model = await _sellerService.GetAll(keyword);
                int totalRow = 0;
                var data = model.OrderByDescending(x => x.SellerId).Skip(page * pageSize).Take(pageSize);
                var mapping = _mapper.Map<IEnumerable<Seller>, IEnumerable<SellerViewModels>>(data);

                totalRow = model.Count();

                var paging = new PaginationSet<SellerViewModels>()
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
        /// lấy seller theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var model = await _sellerService.GetById(id);

                // Kiểm tra nếu không có dữ liệu
                if (model == null)
                {
                    return NotFound(new { message = "Không tìm thấy với ID đã cho." });
                }

                var mapping = _mapper.Map<Seller, SellerViewModels>(model);
                return Ok(mapping);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Đã xảy ra lỗi khi truy vấn dữ liệu.", detail = ex.Message });
            }
        }



        /// <summary>
        /// thêm mới seller
        /// </summary>
        /// <param name="seller"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> Create(SellerViewModels seller)
        {
            if (ModelState.IsValid)
            {
                Seller us = _mapper.Map<SellerViewModels, Seller>(seller);
                try
                {
                    _ = await _sellerService.Add(us);
                    return CreatedAtAction(nameof(Create), new { id = us.SellerId }, us);
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
        /// chỉnh sửa seller
        /// </summary>
        /// <param name="seller"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        public async Task<IActionResult> Update(SellerViewModels seller)
        {
            if (ModelState.IsValid)
            {
                Seller mapping = _mapper.Map<SellerViewModels, Seller>(seller);
                try
                {
                    _ = await _sellerService.Update(mapping);
                    return CreatedAtAction(nameof(Update), new { id = mapping.SellerId }, mapping);
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
        /// xóa seller theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                Seller result = await _sellerService.Delete(id);

                // Kiểm tra nếu không tìm thấy tác giả
                if (result == null)
                {
                    return NotFound(new { message = "Không tìm thấy với ID đã cho." });
                }

                SellerViewModels responseData = _mapper.Map<Seller, SellerViewModels>(result);
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
