using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryAndCartManagement
{
    public class User
    {
        public Guid userId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int WalletAmount { get; set; }

        public List<CartItem> cart;

    }
}
