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
    [Authorize(Roles = nameof(AppRoles.Barista))]
    public class BaristaController(IServiceManger serviceManger) :ApiBaseController
    {
        #region Product
        [HttpPatch("UpdateAvailability/{id}")]
        public async Task<IActionResult> UpdateAvailability(int id, [FromQuery] bool isAvailable)
        {
            var result = await serviceManger.ProductService.UpdateAvailabilityAsync(id, isAvailable);
            return HandleResult(result);
        }

        [HttpGet("GetProductsByCategory/{categoryId}")]
        public async Task<ActionResult<IEnumerable<GetAllProductDTO>>> GetProductsByCategory(int categoryId)
        {
            var result = await serviceManger.ProductService.GetProductsByCategoryAsync(categoryId);
            return HandleResult(result);
        }
        #endregion
    }
}
