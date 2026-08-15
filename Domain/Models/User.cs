using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class User:IdentityUser<int>
    {
        public string FullName { get; set; }=default!;
        public bool IsActive { get; set; } = true;
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
        public ICollection<OrderStatusLog> OrderStatusLog { get; set; } = new List<OrderStatusLog>();
    }
}
