using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    public class OrderWithItemsSpecifications:BaseSpecifications<Order,int>
    {
        public OrderWithItemsSpecifications(int id):base(o=>o.Id==id)
        {
            AddComplexInclude(q => q.Include(i => i.OrderItems).ThenInclude(o => o.Product));
            AddInclude(o => o.Invoice);
        }
    }
}
