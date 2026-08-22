using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.InvoiceDto
{
    public class InvoiceDto
    {
        public int InvoiceId { get; set; }
        public string Status { get; set; }=string.Empty; 
        public string WaiterName { get; set; } = string.Empty;
        public int TableId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
