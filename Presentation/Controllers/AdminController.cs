using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared;
using Shared.DTOS;
using Shared.DTOS.InvoiceDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
   // [Authorize(Roles = nameof(AppRoles.Admin))]
    public class AdminController(IDashboardService _dashboardService, IAuthenticationService _authenticationService) : ApiBaseController
    {

        #region DashboardData
        [HttpGet("GetDashboardData")]
        public async Task<ActionResult<DashboardStatsDto>> GetDashboardData()
        {
            var result = await _dashboardService.GetDashboardStatsAsync();
            return HandleResult(result);
        }
        [HttpPost("ResetDashboardData")]
        public async Task<ActionResult<string>> ResetDashboardData()
        {
            var result = await _dashboardService.ResetDashboardStatsAsync();
            return HandleResult(result);
        }
        #endregion
        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<IEnumerable<GetAllUserDTO>>> GetAllUsers()
        {

            var result = await _authenticationService.GetAllUserAsync();
            return HandleResult(result);
        }


        [HttpPatch("ActiveUser/{id}")]
        public async Task<IActionResult> ActiveUser(int id, [FromQuery] bool isActive)
        {
            var result = await _authenticationService.ActiveUser(id, isActive);
            return HandleResult(result);
        }

    }
}
