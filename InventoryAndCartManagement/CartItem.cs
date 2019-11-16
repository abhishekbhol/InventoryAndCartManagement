using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryAndCartManagement
{
    public class CartItem
    {
        public Guid itemId { get; set; }
        public Product product { get; set; }
        public DateTime createTimestamp { get; set; }

    }
}
