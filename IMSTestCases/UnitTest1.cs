using InventoryAndCartManagement;
using NUnit.Framework;
using System;
using System.Threading;

namespace Tests
{
    public class Tests
    {
        InventoryAndCartService fd;

        [SetUp]
        public void Setup()
        {
            fd = new InventoryAndCartService();
            fd.setUp();
        }

        [Test]
        public void TestAddInventory()
        {
            add2Products();

            var inv = fd.GetInventoryInfo();

            Assert.AreEqual(2, inv.Count);

        }

        [Test]
        public void TestAddTocartBothPositivaAndNegativeCases()
        {
            add2Products();

            fd.AddUser(new User
            {
                Address = "Bangalore",
                Name = "abhi",
                WalletAmount = 6
            });

            var inv = fd.GetInventoryInfo();

            Assert.AreEqual(2, inv.Count);

            var res = fd.AddToCart(inv[0].productId, 6); // 5 product in inventory

            Assert.AreEqual("not enough products in inventory", res);

            res = fd.AddToCart(inv[1].productId, 2);  // Britania is 5 INR

            Assert.AreEqual("not enough amount in wallet", res);

            res = fd.AddToCart(inv[0].productId, 2);

            Assert.AreEqual("succesful", res);

            inv = fd.GetInventoryInfo();

            Assert.AreEqual(inv[0].quantity, 3);

            var cart = fd.GetCartInfo();

            Assert.AreEqual(1, cart.Count);
        }

        [Test]
        public void TestSearchInventory()
        {
            add5Products();

            var inv = fd.GetInventoryInfo();

            Assert.AreEqual(6, inv.Count);

            var res = fd.Search(RatingEnum.five_star);

            Assert.AreEqual(2, res.Count);

            res = fd.Search(BrandsEnum.Amul);

            Assert.AreEqual(1, res.Count);

            res = fd.Search(CategoryEnum.shampoo);

            Assert.AreEqual(2, res.Count);

            res = fd.Search(BrandsEnum.Britania, CategoryEnum.milk);

            Assert.AreEqual(1, res.Count);

        }

        [Test]
        public void TestRemoveFromCart()
        {
            fd.AddInventory(new Product
            {
                brand = BrandsEnum.Amul,
                category = CategoryEnum.milk,
                price = 1,
                quantity = 5,
                rating = RatingEnum.four_star,
                sla = new TimeSpan(0, 0, 1),

            });

            fd.AddUser(new User
            {
                Address = "Bangalore",
                Name = "abhi",
                WalletAmount = 6
            });

            var inv = fd.GetInventoryInfo();

            Assert.AreEqual(1, inv.Count);

            var res = fd.AddToCart(inv[0].productId, 2);

            var cart = fd.GetCartInfo();

            Assert.AreEqual(1, cart.Count);

            inv = fd.GetInventoryInfo();

            Thread.Sleep(2000);

            res = fd.RemoveFromCart(cart[0].itemId);

            Assert.AreEqual("Item SLA crossed", res);

        }


        private void add2Products()
        {
            fd.AddInventory(new Product
            {
                brand = BrandsEnum.Amul,
                category = CategoryEnum.milk,
                price = 1,
                quantity = 5,
                rating = RatingEnum.four_star,
                sla = new TimeSpan(0, 10, 0),

            });

            fd.AddInventory(new Product
            {
                brand = BrandsEnum.Britania,
                category = CategoryEnum.bread,
                price = 5,
                quantity = 6,
                rating = RatingEnum.five_star,
                sla = new TimeSpan(0, 10, 0),

            });
        }

        private void add5Products()
        {
            fd.AddInventory(new Product
            {
                brand = BrandsEnum.Amul,
                category = CategoryEnum.milk,
                price = 1,
                quantity = 5,
                rating = RatingEnum.four_star,
                sla = new TimeSpan(0, 10, 0),

            });

            fd.AddInventory(new Product
            {
                brand = BrandsEnum.Britania,
                category = CategoryEnum.milk,
                price = 1,
                quantity = 5,
                rating = RatingEnum.four_star,
                sla = new TimeSpan(0, 10, 0),

            });

            fd.AddInventory(new Product
            {
                brand = BrandsEnum.Britania,
                category = CategoryEnum.bread,
                price = 5,
                quantity = 6,
                rating = RatingEnum.five_star,
                sla = new TimeSpan(0, 10, 0),

            });

            fd.AddInventory(new Product
            {
                brand = BrandsEnum.ClinicPlus,
                category = CategoryEnum.shampoo,
                price = 3,
                quantity = 7,
                rating = RatingEnum.three_star,
                sla = new TimeSpan(0, 10, 0),

            });

            fd.AddInventory(new Product
            {
                brand = BrandsEnum.HeadAndShoulders,
                category = CategoryEnum.shampoo,
                price = 4,
                quantity = 2,
                rating = RatingEnum.five_star,
                sla = new TimeSpan(0, 10, 0),

            });

            fd.AddInventory(new Product
            {
                brand = BrandsEnum.HideAndSeek,
                category = CategoryEnum.buiscuits,
                price = 1,
                quantity = 5,
                rating = RatingEnum.three_star,
                sla = new TimeSpan(0, 10, 0),

            });
        }
    }
}