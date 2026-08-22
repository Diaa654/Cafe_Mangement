using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.InvoiceDto
{
    public class InvoiceStatusLogDto
    {
        public int OrderId { get; set; }
        public string UserName { get; set; }= string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime ChangedAt { get; set; }
    }
}
