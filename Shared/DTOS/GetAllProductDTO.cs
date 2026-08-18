using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS
{
    public class GetAllProductDTO
    {
        public string Name { get; set; }
        public decimal PriceAfterDiscount { get; set; }
        public bool IsDiscount { get; set; }
        public decimal Discount { get; set; }
        public string ImageURL { get; set; }
    }
}
