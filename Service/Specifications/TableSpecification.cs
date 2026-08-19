using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    public class TableSpecification:BaseSpecifications<Table,int>
    {
        public TableSpecification():base(t=>t.IsAvailable==true)
        {
            
        }
    }
}
