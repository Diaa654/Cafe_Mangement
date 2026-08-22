using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    public class TableWithDetailsSpecifications:BaseSpecifications<Table,int>
    {
        public TableWithDetailsSpecifications():base(null)
        {
            AddComplexInclude(q => q
        .Include(t => t.Invoices.Where(i => i.Status != InvoiceStatus.Paid)
        .OrderByDescending(i=>i.Id).Take(1))
        .ThenInclude(i => i.Orders));
         }
    }
}
