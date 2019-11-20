using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventoryAndCartManagement
{
    public enum RatingEnum
    {
        not_defined,
        one_star,
        two_star,
        three_star,
        four_star,
        five_star,
    }

    public class Rating : SearchBaseClasee
    {
        public override List<Product> Search(List<Product> inventory, params object[] list)
        {
            if (list != null)
            {
                ISearch searchObject = new Rating();
                var criteria = list.Where(x => x is RatingEnum).FirstOrDefault();

                if (criteria != null)
                {
                    if (Enum.TryParse(criteria.ToString(), out RatingEnum r))
                    {
                        return inventory.Where(p => p.rating == r).ToList();
                    }
                }
            }
            return inventory;
        }
    }
}
