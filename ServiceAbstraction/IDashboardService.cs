using Shared.CommonResult;
using Shared.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IDashboardService
    {
        Task<Result<DashboardStatsDto>> GetDashboardStatsAsync();
        Task<Result<string>> ResetDashboardStatsAsync();
    }
}
