using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.InvoiceDto
{
    public enum OrderStatus
    {
        Waiting,
        Preparing,
        Ready,
        Delivered,
        Completed,
        Canceled
    }
}
