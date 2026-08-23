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
    public class ProductController(IServiceManger serviceManger) : ApiBaseController
    {
        #region Product
        [Authorize(Roles = nameof(AppRoles.Admin))]
        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct([FromForm] AddProductDTO dto)
        {
            var result = await serviceManger.ProductService.AddAsync(dto);
            return HandleResult(result);
        }
        [Authorize(Roles = nameof(AppRoles.Admin))]
        [HttpDelete("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await serviceManger.ProductService.DeleteAsync(id);
            return HandleResult(result);
        }
        [Authorize(Roles = nameof(AppRoles.Admin))]
        [HttpPut("UpdateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto dto)
        {
            var result = await serviceManger.ProductService.UpdateAsync(id, dto);
            return HandleResult(result);
        }
        [Authorize(Roles = nameof(AppRoles.Admin))]
        [HttpPatch("UpdateDiscount/{id}")]
        public async Task<IActionResult> UpdateDiscount(int id, [FromQuery] decimal newDiscount)
        {
            var result = await serviceManger.ProductService.UpdateDiscountAsync(id, newDiscount);
            return HandleResult(result);
        }
        [Authorize(Roles = nameof(AppRoles.Admin))]
        [HttpPatch("UpdateImageProduct/{id}")]
        public async Task<IActionResult> UpdateImage(int id, IFormFile image)
        {
            var result = await serviceManger.ProductService.UpdateImageProductAsync(id, image);
            return HandleResult(result);
        }
        [Authorize(Roles = nameof(AppRoles.Waiter) + "," + nameof(AppRoles.Admin))]
        [HttpGet("GetProductsByCategory/{categoryId}")]
        public async Task<ActionResult<IEnumerable<GetAllProductDTO>>> GetProductsByCategory(int categoryId)
        {
            var result = await serviceManger.ProductService.GetProductsByCategoryAsync(categoryId);
            return HandleResult(result);
        }
        [Authorize(Roles = nameof(AppRoles.Admin))]
        [HttpGet("GetTopProducts")]
        public async Task<ActionResult<IEnumerable<GetTopProductDTO>>> GetTopProducts()
        {
            var result = await serviceManger.ProductService.GetTopProductsAsync();
            return HandleResult(result);
        }
        #endregion
        [Authorize(Roles = nameof(AppRoles.Barista))]
        #region Product
        [HttpPatch("UpdateAvailabilityProduct/{id}")]
        public async Task<IActionResult> UpdateAvailability(int id, [FromQuery] bool isAvailable)
        {
            var result = await serviceManger.ProductService.UpdateAvailabilityAsync(id, isAvailable);
            return HandleResult(result);
        }

      
        #endregion
    }
}
