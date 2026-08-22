using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; } = 0;
        public Order Order { get; set; } = default!;
        public Product Product { get; set; } = default!;
        public decimal TotalPrice => (UnitPrice - (UnitPrice * (Discount / 100m))) * Quantity;
    }
}
