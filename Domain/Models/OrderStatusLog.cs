using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class OrderStatusLog
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = default!;//added
        public OrderStatus Status { get; set; }
        public DateTime ChangedAt { get; set; }
    }
}
