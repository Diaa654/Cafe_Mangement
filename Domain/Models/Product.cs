using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; } = true;
        public decimal Discount { get; set; }
        public string ImageUrl { get; set; } = default!;
        public int NumberOfSales { get; set; } = 0;
        public int CategoryId { get; set; }
        public Category Category { get; set; } = default!;
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
