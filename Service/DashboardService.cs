using Domain.Contracts;
using ServiceAbstraction;
using Shared.CommonResult;
using Shared.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class DashboardService(ICacheRepository _cacheRepository) : IDashboardService
    {
       
        public async Task<Result<DashboardStatsDto>> GetDashboardStatsAsync()
        {
            
            var totalAmount = await _cacheRepository.GetAsync("Total_financial_collection");
            var totalInvoices = await _cacheRepository.GetAsync("Total_invoices_count");

            var stats = new DashboardStatsDto
            {
                TotalCollection = totalAmount,
                TotalInvoicesCount = (int)totalInvoices
            };

            return Result<DashboardStatsDto>.Ok(stats);
        }

        public async Task<Result<string>> ResetDashboardStatsAsync()
        {
            
            await _cacheRepository.ResetToZeroAsync("Total_financial_collection");
            await _cacheRepository.ResetToZeroAsync("Total_invoices_count");

            return Result<string>.Ok("تم تصفير العدادات بنجاح.");
        }
    }
}
