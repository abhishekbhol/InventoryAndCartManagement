using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventoryAndCartManagement
{
    public interface ISearch
    {
        List<Product> Search(List<Product> inventory, params object[] list);
    }

    public abstract class SearchBaseClasee : ISearch
    {
        public abstract List<Product> Search(List<Product> inventory, params object[] list);
    }

    public class SearchImpl
    {
        private List<SearchBaseClasee> observers = new List<SearchBaseClasee>();

        public void attach(SearchBaseClasee observer)
        {
            observers.Add(observer);
        }

        public List<Product> Search(List<Product> res, params object[] list)
        {
            foreach(var observer in observers)
            {
                res = observer.Search(res, list);
            }

            return res;
        }
    }
}
