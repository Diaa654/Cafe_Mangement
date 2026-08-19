using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared;
using Shared.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Authorize(Roles = nameof(AppRoles.Admin))]
    public class AdminController(IServiceManger serviceManger) : ApiBaseController
    {
        #region Category
        [HttpPost("AddCategory")]
        public async Task<IActionResult> Add([FromBody] CategoryDTO dto)
        {
            var result = await serviceManger.CategoryService.AddAsync(dto);
            return HandleResult(result);
        }
        
        [HttpPut("UpdateCategory/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryDTO dto)
        {
            var result = await serviceManger.CategoryService.UpdateAsync(id, dto);
            return HandleResult(result);
        }

        [HttpDelete("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await serviceManger.CategoryService.DeleteAsync(id);
            return HandleResult(result);
        }
        [HttpGet("GetAllCategories")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAll()
        {
            var result = await serviceManger.CategoryService.GetAllCategoriesAsync();
            return HandleResult(result);
        }
        #endregion

        #region Product

        [HttpPost("AddProduct")]
        public async Task<IActionResult> Add([FromForm] AddProductDTO dto)
        {
            var result = await serviceManger.ProductService.AddAsync(dto);
            return HandleResult(result);
        }


        [HttpDelete("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await serviceManger.ProductService.DeleteAsync(id);
            return HandleResult(result);
        }

        [HttpPut("UpdateProduct/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto)
        {
            var result = await serviceManger.ProductService.UpdateAsync(id, dto);
            return HandleResult(result);
        }

        [HttpPatch("UpdateDiscount/{id}")]
        public async Task<IActionResult> UpdateDiscount(int id, [FromQuery] decimal newDiscount)
        {
            var result = await serviceManger.ProductService.UpdateDiscountAsync(id, newDiscount);
            return HandleResult(result);
        }
        [HttpPatch("UpdateImageProduct/{id}")]
        public async Task<IActionResult> UpdateImage(int id, [FromForm] IFormFile image)
        {
            var result = await serviceManger.ProductService.UpdateImageProductAsync(id, image);
            return HandleResult(result);
        }

        [HttpGet("GetProductsByCategory/{categoryId}")]
        public async Task<ActionResult<IEnumerable<GetAllProductDTO>>> GetProductsByCategory(int categoryId)
        {
            var result = await serviceManger.ProductService.GetProductsByCategoryAsync(categoryId);
            return HandleResult(result);
        }

        [HttpGet("GetTopProducts")]
        public async Task<ActionResult<IEnumerable<GetTopProductDTO>>> GetTopProducts()
        {
            var result = await serviceManger.ProductService.GetTopProductsAsync();
            return HandleResult(result);
        }
        #endregion

        #region Table 

        [HttpPost("AddTable")]
        public async Task<IActionResult> AddTable()
        {
            var result = await serviceManger.TableService.AddTable();
            return HandleResult(result);
        }
        #endregion
    }
}
