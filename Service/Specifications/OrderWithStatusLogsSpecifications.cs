using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    internal class OrderWithStatusLogsSpecifications:BaseSpecifications<Order, int>
    {
        public OrderWithStatusLogsSpecifications(int id) : base(o => o.Id == id)
        {
            AddInclude(o => o.StatusLogs);
            AddComplexInclude(q=>q.Include(o => o.Invoice)
                                        .ThenInclude(oi => oi.Table));
        }
    }
}
