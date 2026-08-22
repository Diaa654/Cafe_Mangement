using Microsoft.AspNetCore.SignalR;
using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Hubs
{
    public class DashboardNotificationService(IHubContext<AdminDashboardHub> _adminHub) : IDashboardNotificationService
    {
        public async Task SendDashboardUpdatesAsync(double totalCollection, int totalInvoicesCount)
        {
            
            await _adminHub.Clients.All.SendAsync("ReceiveDashboardUpdates", new
            {
                TotalCollection = totalCollection,
                TotalInvoicesCount = totalInvoicesCount
            });
        }
    }
}
