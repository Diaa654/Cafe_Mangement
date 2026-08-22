using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.InvoiceDto
{
    public class InvoiceDetailsDto
    {
        public int InvoiceId { get; set; }
        public string WaiterName { get; set; } = string.Empty;
        public int TableId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string InvoiceStatus { get; set; }=string.Empty;
        public decimal TotalAmount { get; set; }
        public List<OrderBasicDto> Orders { get; set; } = new();
    }
}
