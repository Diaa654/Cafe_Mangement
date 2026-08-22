using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Table
    {
        public int Id { get; set; }
        public string TableNumber { get; set; } = default!;
        public bool IsAvailable { get; set; } = true;
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}
