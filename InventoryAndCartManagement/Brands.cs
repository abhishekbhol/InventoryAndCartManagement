using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventoryAndCartManagement
{
    public enum BrandsEnum
    {
        Amul,
        Britania,
        HeadAndShoulders,
        ClinicPlus,
        GoodDay,
        HideAndSeek,
    }

    public class Brands : SearchBaseClasee
    {
        public override List<Product> Search(List<Product> inventory, params object[] list)
        {
            if(list != null)
            {
                ISearch searchObject = new Brands();
                var criteria = list.Where(x => x is BrandsEnum).FirstOrDefault();

                if (criteria != null)
                {
                    if (Enum.TryParse(criteria.ToString(), out BrandsEnum b))
                    {
                        return inventory.Where(p => p.brand == b).ToList();
                    }
                }
            }
            return inventory;
        }

    }
}
