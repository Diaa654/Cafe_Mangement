using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    internal class BaristaOrdersSpecifications:BaseSpecifications<Order, int>
    {
        public BaristaOrdersSpecifications() : base(o => o.Status == OrderStatus.Waiting || o.Status == OrderStatus.Preparing)
        {
            AddComplexInclude(q => q.Include(i => i.OrderItems).ThenInclude(o => o.Product));
            AddInclude(o => o.Invoice);
            AddOrderBy(o => (DateTime?)o.CreatedAt);
        }
    }
}
