using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventoryAndCartManagement
{
    public enum CategoryEnum
    {
        milk,
        bread,
        shampoo,
        buiscuits,
    }

    public class Category : SearchBaseClasee
    {
        public override List<Product> Search(List<Product> inventory, params object[] list)
        {
            if (list != null)
            {
                ISearch searchObject = new Category();
                var criteria = list.Where(x => x is CategoryEnum).FirstOrDefault();

                if (criteria != null)
                {
                    if (Enum.TryParse(criteria.ToString(), out CategoryEnum b))
                    {
                        return inventory.Where(p => p.category == b).ToList();
                    }
                }
            }
            return inventory;
        }

    }
}
