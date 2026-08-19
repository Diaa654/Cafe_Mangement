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
    [Authorize(Roles = nameof(AppRoles.Waiter))]
    public class WaiterController(IServiceManger serviceManger) : ApiBaseController
    {
        #region Table
        [HttpGet("GetAllTables")]
        public async Task<ActionResult<IEnumerable<GetAllTableDTO>>> GetAllTables()
        {
            var result = await serviceManger.TableService.GetAllTablesAsync();
            return HandleResult(result);
        }
        [HttpGet("GetAllTableAvailables")]
        public async Task<ActionResult<IEnumerable<GetAllTableDTO>>> GetAllTableAvailables()
        {
            var result = await serviceManger.TableService.GetAllTableAvailablesAsync();
            return HandleResult(result);
        }

        [HttpPatch("UpdateTableAvailability/{tableId}")]
        public async Task<IActionResult> UpdateTableAvailability(int tableId, [FromQuery] bool isAvailable)
        {
            var result = await serviceManger.TableService.UpdateTableAvailability(tableId, isAvailable);
            return HandleResult(result);
        }
        #endregion
    }
}
