using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.Order
{
    public class UpdateOrderDto
    {
        public List<UpdateOrderItemDto> Items { get; set; } = new();
    }
}
