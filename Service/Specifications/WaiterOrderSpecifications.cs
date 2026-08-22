using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    public class WaiterOrderSpecifications:BaseSpecifications<Order, int>
    {
        public WaiterOrderSpecifications() : base(o => o.Status == OrderStatus.Ready)
        {
            AddComplexInclude(q => q.Include(i => i.OrderItems).ThenInclude(o => o.Product));
            AddInclude(o => o.Invoice);
            AddOrderBy(o => (DateTime?)o.CreatedAt);
        }
    }
}
