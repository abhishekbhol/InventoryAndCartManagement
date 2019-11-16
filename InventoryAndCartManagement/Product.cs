using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryAndCartManagement
{
    public class Product
    {
        public Guid productId { get; set; }
        public Category category { get; set; }
        public Brands brand { get; set; }
        public int quantity { get; set; }
        public int price { get; set; }
        public Rating rating { get; set; }
        public TimeSpan sla { get; set; }
    }
}
