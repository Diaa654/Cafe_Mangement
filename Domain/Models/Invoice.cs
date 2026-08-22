using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public InvoiceStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
        public int UserId { get; set; }
        public int TableId { get; set; }
        public Table Table { get; set; } = default!;
        public User User { get; set; } = default!;
        public ICollection<Order> Orders { get; set; } = new List<Order>();

    }
}
