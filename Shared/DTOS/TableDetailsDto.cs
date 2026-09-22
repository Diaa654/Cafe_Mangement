using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS
{
    public class TableDetailsDto
    {
        
        public int TableId { get; set; }
        public int InvoiceId { get; set; } = 0;
        public string OrderStatus { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
    }
}
