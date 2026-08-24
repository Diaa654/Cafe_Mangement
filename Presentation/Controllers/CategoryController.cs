using Microsoft.AspNetCore.Authorization;
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
    public class CategoryController(IServiceManger serviceManger):ApiBaseController
    {
        #region Category
        [Authorize(Roles = nameof(AppRoles.Admin))]
        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDTO dto)
        {
            var result = await serviceManger.CategoryService.AddAsync(dto);
            return HandleResult(result);
        }
        [Authorize(Roles = nameof(AppRoles.Admin))]
        [HttpPut("UpdateCategory/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDTO dto)
        {
            var result = await serviceManger.CategoryService.UpdateAsync(id, dto);
            return HandleResult(result);
        }
        [Authorize(Roles = nameof(AppRoles.Admin))]
        [HttpDelete("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await serviceManger.CategoryService.DeleteAsync(id);
            return HandleResult(result);
        }
        [Authorize(Roles = nameof(AppRoles.Waiter) + "," + nameof(AppRoles.Admin))]
        [HttpGet("GetAllCategories")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAllCategories()
        {
            var result = await serviceManger.CategoryService.GetAllCategoriesAsync();
            return HandleResult(result);
        }
        #endregion
    }
}
