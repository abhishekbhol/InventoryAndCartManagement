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
        public SearchImpl search;

        public void setUp()
        {
            inventory = new List<Product>();
            userList = new List<User>();
            search = new SearchImpl();
            search.attach(new Brands());
            search.attach(new Category());
            search.attach(new Rating());
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

                if (prod.rating == RatingEnum.not_defined)
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
        /// <param name="list"></param>
        /// <returns></returns>
        public List<Product> Search(params object[] list)
        {
            return search.Search(inventory, list);
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
