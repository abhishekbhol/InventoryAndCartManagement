using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventoryAndCartManagement
{
    public class InventoryAndCartService
    {
        public List<Product> inventory;
        public List<User> userList;
        public User activeUser;

        public void setUp()
        {
            inventory = new List<Product>();
            userList = new List<User>();
        }

        /// <summary>
        /// Add inventory
        /// </summary>
        /// <param name="prod"></param>
        /// <returns></returns>
        public bool AddInventory(Product prod)
        {
            var prodExists = inventory.Where<Product>(p => p.brand == prod.brand && p.category == prod.category).FirstOrDefault();

            if(prodExists != null)
            {
                // update the price and rating of order batch
                prod.quantity = prod.quantity + prodExists.quantity;

                if (prod.rating == Rating.not_defined)
                    prod.rating = prodExists.rating;

                if (prod.sla == null)
                    prod.sla = prodExists.sla;

                // removing order batch
                inventory.Remove(prodExists);
            }

            prod.productId = Guid.NewGuid();

            inventory.Add(prod);

            return true;
        }

        /// <summary>
        /// Adding a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string AddUser(User user)
        {
            user.userId = Guid.NewGuid();
            user.cart = new List<CartItem>();

            if (user.WalletAmount <= 0)
            {
                return "please add amount in wallet";
            }

            userList.Add(user);
            activeUser = user;

            return "success";
        }

        public void ChangeActiveUser(Guid userId)
        {
            var user = userList.Where(a => a.userId == userId).FirstOrDefault();

            if (user == null)
                return;

            activeUser = user;
        }

        /// <summary>
        /// Searching on the basis of Brand, Category, Rating and Quantity
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public List<Product> Search(string criteria)
        {

            if (Enum.TryParse(criteria, out Brands b))
            {
                return inventory.Where(p => p.brand == b).ToList();
            }

            if (Enum.TryParse(criteria, out Category c))
            {
                return inventory.Where(p => p.category == c).ToList();
            }

            if (Enum.TryParse(criteria, out Rating res))
            {
                return inventory.Where(p => p.rating == res).ToList();
            }

            int quantity = 0;

            if (Int32.TryParse(criteria, out quantity))
            {
                return inventory.Where(p => p.quantity == quantity).ToList();
            }

            return null;

        }

        /// <summary>
        /// Adding a product to cart
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public string AddToCart(Guid productId, int quantity = 1)
        {

            var product = inventory.Where(p => p.productId == productId).FirstOrDefault();

            if(activeUser.WalletAmount < (product.price * quantity))
            {
                return "not enough amount in wallet";
            }

            if(product.quantity < quantity)
            {
                return "not enough products in inventory";
            }

            var cartProduct = new Product
            {
                brand = product.brand,
                category = product.category,
                price = product.price,
                productId = product.productId,
                quantity = quantity,
                rating = product.rating,
                sla = product.sla
            };

            var cartItem = new CartItem
            {
                product = cartProduct,
                itemId = Guid.NewGuid(),
                createTimestamp = DateTime.Now
            };

            activeUser.cart.Add(cartItem);
            product.quantity -= quantity;

            return "succesful";
        }

        public string RemoveFromCart(Guid itemId)
        {
            var item = activeUser.cart.Where(a => a.itemId == itemId).FirstOrDefault();

            if(item == null)
            {
                return "no item exists";
            }

            if(DateTime.Now - item.createTimestamp > item.product.sla)
            {
                return "Item SLA crossed";
            }

            activeUser.cart.Remove(item); // update invemtory

            return "success";
        }

        public List<CartItem> GetCartInfo()
        {
            return activeUser.cart;
        }

        public List<Product> GetInventoryInfo()
        {
            return inventory;
        }


        private void ClearCart()
        {
            activeUser.cart = new List<CartItem>();
        }
    }
}
