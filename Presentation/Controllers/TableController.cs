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
    public class TableController(IServiceManger _serviceManager):ApiBaseController
    {
        [Authorize(Roles = nameof(AppRoles.Waiter) + "," + nameof(AppRoles.Barista))]
        [HttpGet("GetAllTablesWithDetails")]
        public async Task<ActionResult<IEnumerable<TableDetailsDto>>> GetAllTablesWithDetails()
        {
            var userId = GetUserId();
            var result = await _serviceManager.TableService.GetAllTablesWithDetails(userId);
            return HandleResult(result);
        }


        #region Table


        [Authorize(Roles = nameof(AppRoles.Admin))]
        [HttpPost("AddTable")]
        public async Task<IActionResult> AddTable()
        {
            var result = await _serviceManager.TableService.AddTable();
            return HandleResult(result);
        }
        [Authorize(Roles = nameof(AppRoles.Waiter) + "," + nameof(AppRoles.Barista))]
        [HttpGet("GetAllTables")]
        public async Task<ActionResult<IEnumerable<GetAllTableDTO>>> GetAllTables()
        {
            var result = await _serviceManager.TableService.GetAllTablesAsync();
            return HandleResult(result);
        }
        [Authorize(Roles = nameof(AppRoles.Waiter) + "," + nameof(AppRoles.Barista))]
        [HttpGet("GetAllTableAvailables")]
        public async Task<ActionResult<IEnumerable<GetAllTableDTO>>> GetAllTableAvailables()
        {
            var result = await _serviceManager.TableService.GetAllTableAvailablesAsync();
            return HandleResult(result);
        }
        [Authorize(Roles = nameof(AppRoles.Waiter) )]
        [HttpPatch("UpdateTableAvailability/{tableId}")]
        public async Task<IActionResult> UpdateTableAvailability(int tableId, [FromQuery] bool isAvailable)
        {
            var result = await _serviceManager.TableService.UpdateTableAvailability(tableId, isAvailable);
            return HandleResult(result);
        }
        #endregion
    }
}
