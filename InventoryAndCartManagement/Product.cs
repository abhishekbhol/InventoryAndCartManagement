using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryAndCartManagement
{
    public class Product
    {
        public Guid productId { get; set; }
        public CategoryEnum category { get; set; }
        public BrandsEnum brand { get; set; }
        public int quantity { get; set; }
        public int price { get; set; }
        public RatingEnum rating { get; set; }
        public TimeSpan sla { get; set; }
    }
}
