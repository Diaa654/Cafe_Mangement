using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    public class InvoiceDetailsSpecifications:BaseSpecifications<Invoice,int>
    {
        public InvoiceDetailsSpecifications(int invoiceId) : base(i => i.Id == invoiceId)
        {
            AddInclude(i => i.User);
           // AddComplexInclude(q => q.Include(i => i.Orders).ThenInclude(o => o.OrderItems));
            AddComplexInclude(q => q.Include(i => i.Orders).ThenInclude(o => o.StatusLogs));
            AddComplexInclude(q => q.Include(i => i.Orders).ThenInclude(o => o.OrderItems).ThenInclude(oi => oi.Product));

        }
    }
}
