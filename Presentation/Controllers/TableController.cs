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
        [HttpGet("GetAllTablesWithDetails")]
        [Authorize(Roles = nameof(AppRoles.Waiter))]
        public async Task<ActionResult<IEnumerable<TableDetailsDto>>> GetAllTablesWithDetails()
        {
            var userId = GetUserId();
            var result = await _serviceManager.TableService.GetAllTablesWithDetails(userId);
            return HandleResult(result);
        }
    }
}
