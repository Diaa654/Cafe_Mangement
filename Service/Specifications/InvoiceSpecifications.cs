
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Shared.DTOS.InvoiceDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvoiceStatus = Domain.Models.InvoiceStatus;

namespace Service.Specifications
{
    public class InvoiceSpecifications : BaseSpecifications<Invoice, int>
    {
        public InvoiceSpecifications(int TableId) : base(i => i.TableId == TableId && i.Status == InvoiceStatus.Pending)
        {
            AddInclude(i => i.User);
            AddComplexInclude(q => q.Include(i => i.Orders).ThenInclude(o => o.OrderItems).ThenInclude(oi => oi.Product));

        }
        public InvoiceSpecifications(InvoiceSpecParams specParams) : base(i =>
            (!specParams.StartDate.HasValue || i.Orders.Min(o => (DateTime?)o.CreatedAt) >= specParams.StartDate) &&
            (!specParams.EndDate.HasValue || i.Orders.Min(o => (DateTime?)o.CreatedAt) <= specParams.EndDate))
        {
            AddInclude(i => i.User);
            AddComplexInclude(q => q.Include(i => i.Orders).ThenInclude(o => o.OrderItems));
            AddOrderByDescending(i => i.Orders.Min(o => (DateTime?)o.CreatedAt));
            ApplyPaging(specParams.PageSize,specParams.PageIndex);
        }
    }
}
