using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS
{
    public class GetTopProductDTO
    {
        public string Name { get; set; } 
        public string ImageUrl { get; set; }
        public int NumberOfSales { get; set; }

    }
}
