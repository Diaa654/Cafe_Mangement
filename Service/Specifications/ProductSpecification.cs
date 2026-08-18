using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    public class ProductSpecification:BaseSpecifications<Product,int>
    {
        public ProductSpecification(int CategoryId):base(p =>p.CategoryId == CategoryId)
        {
            
        }
        public ProductSpecification(): base(p => true)
        {
            AddOrderByDescending(p => p.NumberOfSales);
        }
    }
}
