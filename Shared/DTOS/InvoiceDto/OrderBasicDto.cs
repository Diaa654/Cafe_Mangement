using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.InvoiceDto
{
    public class OrderBasicDto
    {
        public int OrderId { get; set; }
        public string OrderStatus { get; set; }=string.Empty;
        public DateTime CreatedAt { get; set; }
        public decimal OrderTotal { get; set; }
        public int TableId { get; set; }
        public ICollection<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
        
    }
}
