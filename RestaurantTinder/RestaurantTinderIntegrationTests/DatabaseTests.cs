using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestaurantTinder.Interfaces;
using System;
using RestaurantTinder.Models;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using RestaurantService.Exceptions;
using RestaurantTinder.Database;
using RestaurantService;

namespace RestaurantTinderIntegrationTests
{
    [TestClass]
    public class DatabaseTests
    {
        //Used to begin a transaction during initialize and rollback during cleanup
        private const string _connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=RestaurantTinder;Integrated Security=true";
        private TransactionScope _tran = null;
        private IRestaurantService _db = null;

        private int _restaurantId = BaseItem.InvalidId;
        private int _preferredFoodId = BaseItem.InvalidId;
        private int _favoritesId = BaseItem.InvalidId;
        private int _blacklistId = BaseItem.InvalidId;
        private int _zip = BaseItem.InvalidZip;
        private int _userId1 = BaseItem.InvalidId;
        private int _userId2 = BaseItem.InvalidId;

        /// <summary>
        /// Set up the database before each test  
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            _db = new RestaurantDBService(_connectionString);
       

            // Initialize a new transaction scope. This automatically begins the transaction.
            _tran = new TransactionScope();

            PasswordManager passHelper = new PasswordManager("Abcd!234");
            if (_userId1 == BaseItem.InvalidId)
            {
                var temp = new UserItem() { Id = BaseItem.InvalidId };
                temp.FirstName = "Garrick";
                temp.LastName = "Kreitzer";
                temp.Username = "gkreitzer";
                temp.Hash = passHelper.Hash;
                temp.Salt = passHelper.Salt;
                temp.Email = "garrickkreitzer1@gmail.com";
                temp.RoleId = (int)RoleManager.eRole.Customer;

                // Add user item
                _userId1 = _db.AddUserItem(temp);
                Assert.AreNotEqual(0, _userId1);
            }

            if (_userId2 == BaseItem.InvalidId)
            {
                var temp = new UserItem() { Id = BaseItem.InvalidId };
                temp.FirstName = "Thomas";
                temp.LastName = "Martinez";
                temp.Username = "tdiddy";
                temp.Hash = passHelper.Hash;
                temp.Salt = passHelper.Salt;
                temp.Email = "garrickkreitzer1@gmail.com";
                temp.RoleId = RoleManager.eRole.Customer;

                // Add user item
                _userId2 = _db.AddUserItem(temp);
                Assert.AreNotEqual(0, _userId2);
            }

            if (_restaurantId == BaseItem.InvalidId)
            {
                var temp = new RestaurantItem() { Id = BaseItem.InvalidId };
                temp.Name = "TestRestaurant";

                // Add restaurant item
                _restaurantId = _db.AddRestaurantItem(temp);
                Assert.AreNotEqual(0, _restaurantId);
            }

            if (_preferredFoodId == BaseItem.InvalidId)
            {
                // Add preferred food item
                _preferredFoodId = _db.AddPreferredFoodItem(
                new PreferredFoodItem()
                {
                    FoodItem = "TestPreferredFood",
                    UserId = _userId1,
                });
                Assert.AreNotEqual(0, _preferredFoodId);
            }

            if (_favoritesId == BaseItem.InvalidId)
            {
                // Add favorites item
                _favoritesId = _db.AddFavoritesItem(
                new FavoritesItem()
                {
                    RestaurantId = _restaurantId,
                    UserId = _userId1
                });
                Assert.AreNotEqual(0, _favoritesId);
            }

            if (_blacklistId == BaseItem.InvalidId)
            {
                // Add blacklist item
                _blacklistId = _db.AddBlacklistItem(
                new BlacklistItem()
                {
                    RestaurantId = _restaurantId,
                    UserId = _userId1
                });
                Assert.AreNotEqual(0, _blacklistId);
            }

            //Need to be able to test ZipItem
        }

        /// <summary>
        /// Cleanup runs after every single test
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            _tran.Dispose(); //<-- disposing the transaction without committing it means it will get rolled back
            _restaurantId = BaseItem.InvalidId;
            _preferredFoodId = BaseItem.InvalidId;
            _favoritesId = BaseItem.InvalidId;
            _blacklistId = BaseItem.InvalidId;
            _zip = BaseItem.InvalidZip;
        }

        /// <summary>
        /// Tests the user POCO methods
        /// </summary>
        [TestMethod()]
        public void TestUser()
        {
            PasswordManager passHelper = new PasswordManager("Abcd!234");

            // Test add user
            UserItem item = new UserItem();
            item.FirstName = "Garrick";
            item.LastName = "Kreitzer";
            item.Username = "gkreitzer";
            item.Hash = passHelper.Hash;
            item.Salt = passHelper.Salt;
            item.Email = "garrickkreitzer1@gmail.com";
            item.RoleId = (int)RoleManager.eRole.Customer;
            int id = _db.AddUserItem(item);
            Assert.AreNotEqual(0, id);

            UserItem itemGet = _db.GetUserItem(id);
            Assert.AreEqual(item.Id, itemGet.Id);
            Assert.AreEqual(item.FirstName, itemGet.FirstName);
            Assert.AreEqual(item.LastName, itemGet.LastName);
            Assert.AreEqual(item.Username, itemGet.Username);
            Assert.AreEqual(item.Hash, itemGet.Hash);
            Assert.AreEqual(item.Salt, itemGet.Salt);
            Assert.AreEqual(item.Email, itemGet.Email);

            // Test update user
            item.FirstName = "What";
            item.LastName = "What";
            item.Username = "What";
            item.Email = "What";
            item.Hash = "What";
            item.Salt = "What";
            Assert.IsTrue(_db.UpdateUserItem(item));

            itemGet = _db.GetUserItem(id);
            Assert.AreEqual(item.Id, itemGet.Id);
            Assert.AreEqual(item.FirstName, itemGet.FirstName);
            Assert.AreEqual(item.LastName, itemGet.LastName);
            Assert.AreEqual(item.Username, itemGet.Username);
            Assert.AreEqual(item.Hash, itemGet.Hash);
            Assert.AreEqual(item.Salt, itemGet.Salt);
            Assert.AreEqual(item.Email, itemGet.Email);

            // Test delete user
            _db.DeleteUserItem(id);
            var users = _db.GetUserItems();
            foreach (var user in users)
            {
                Assert.AreNotEqual(id, user.Id);
            }
        }

        /// <summary>
        /// Tests the restaurant POCO methods
        /// </summary>
        [TestMethod()]
        public void TestRestaurant()
        {
            // Test add restuarant
            RestaurantItem item = new RestaurantItem();
            item.Id = _restaurantId;
            item.Name = "Poophouse";
            int id = _db.AddRestaurantItem(item);
            Assert.AreNotEqual(0, id);

            RestaurantItem itemGet = _db.GetRestaurantItem("Poophouse");
            Assert.AreEqual(item.Id, itemGet.Id);
            Assert.AreEqual(item.Name, itemGet.Name);

            // Test update restaurant
            item.Name = "Howdy";
            Assert.IsTrue(_db.UpdateRestaurantItem(item));

            itemGet = _db.GetRestaurantItem("Howdy");
            Assert.AreEqual(item.Id, itemGet.Id);
            Assert.AreEqual(item.Name, itemGet.Name);

            // Test delete restaurant
            _db.DeleteRestaurantItem("Howdy");
            var restaurants = _db.GetRestaurantItems("Howdy");
            foreach (var restaurant in restaurants)
            {
                Assert.AreNotEqual(id, restaurant.Id);
            }
        }

        /// <summary>
        /// Tests the preferred food POCO methods
        /// </summary>
        [TestMethod()]
        public void TestPreferredFood()
        {
            // Test add preferred food
            PreferredFoodItem item = new PreferredFoodItem();
            item.FoodItem = "Snot";
            item.UserId = _userId1;
            int id = _db.AddPreferredFoodItem(item);
            Assert.AreNotEqual(0, id);

            PreferredFoodItem itemGet = _db.GetPreferredFoodItem(id);
            Assert.AreEqual(item.FoodItem, itemGet.FoodItem);
            Assert.AreEqual(item.UserId, itemGet.UserId);

            // Test delete preferred food
            _db.DeletePreferredFoodItem(item.UserId, item.FoodItem);
            var foods = _db.GetPreferredFoodItems(item.UserId);
            foreach (var food in foods)
            {
                Assert.AreNotEqual(id, food.Id);
            }
        }

        /// <summary>
        /// Tests the favorites POCO methods
        /// </summary>
        [TestMethod()]
        public void TestFavorites()
        {
            // Test add favorites
            FavoritesItem item = new FavoritesItem();
            item.Id = _favoritesId;
            int id = _db.AddFavoritesItem(item);
            Assert.AreNotEqual(0, id);

            FavoritesItem itemGet = _db.GetFavoritesItem(item.UserId, item.RestaurantId);
            Assert.AreEqual(item.Id, itemGet.Id);
            Assert.AreEqual(item.UserId, itemGet.UserId);
            Assert.AreEqual(item.RestaurantId, itemGet.RestaurantId);

            // Test delete favorites
            _db.DeleteFavoritesItem(item.UserId, item.RestaurantId);
            var favoritesItems = _db.GetFavoritesItems(item.UserId);
            foreach (var favoritesItem in favoritesItems)
            {
                Assert.AreNotEqual(id, favoritesItem.Id);
            }
        }

        /// <summary>
        /// Tests the vending transaction POCO methods
        /// </summary>
        [TestMethod()]
        public void TestVendingTransaction()
        {
            // Test add vending transaction
            VendingTransaction item = new VendingTransaction();
            item.Date = DateTime.UtcNow;
            item.UserId = _userId1;
            int id = _db.AddVendingTransaction(item);
            Assert.AreNotEqual(0, id);

            VendingTransaction itemGet = _db.GetVendingTransaction(id);
            Assert.AreEqual(item.Id, itemGet.Id);
            Assert.AreEqual(item.UserId, itemGet.UserId);
            Assert.AreEqual(item.Date.ToString(), itemGet.Date.ToString());

            // Test get vending transactions
            item = new VendingTransaction();
            item.Date = DateTime.UtcNow;
            item.UserId = _userId2;
            int id2 = _db.AddVendingTransaction(item);
            Assert.AreNotEqual(0, id2);

            var vendingItems = _db.GetVendingTransactions();
            bool foundItem1 = false;
            bool foundItem2 = false;
            foreach (var vendingItem in vendingItems)
            {
                if (vendingItem.Id == id)
                {
                    foundItem1 = true;
                }
                else if (vendingItem.Id == id2)
                {
                    foundItem2 = true;
                }
            }
            Assert.IsTrue(foundItem1);
            Assert.IsTrue(foundItem2);
        }

        /// <summary>
        /// Tests the transaction item POCO methods
        /// </summary>
        [TestMethod()]
        public void TestTransactionItems()
        {
            // Test add transaction item
            TransactionItem item = new TransactionItem();
            item.ProductId = _productId;
            item.VendingTransactionId = _vendingTransactionId;
            item.SalePrice = 0.46;
            int id = _db.AddTransactionItem(item);
            Assert.AreNotEqual(0, id);

            TransactionItem itemGet = _db.GetTransactionItem(id);
            Assert.AreEqual(item.Id, itemGet.Id);
            Assert.AreEqual(item.ProductId, itemGet.ProductId);
            Assert.AreEqual(item.VendingTransactionId, itemGet.VendingTransactionId);
            Assert.AreEqual(item.SalePrice.ToString("c"), itemGet.SalePrice.ToString("c"));

            // Test get transaction items
            var transactionItems = _db.GetTransactionItems();
            bool foundItem = false;
            foreach (var transactionItem in transactionItems)
            {
                if (transactionItem.Id == id)
                {
                    foundItem = true;
                }
            }
            Assert.IsTrue(foundItem);

            // Test get transaction items by vending transaction id
            transactionItems = _db.GetTransactionItems(_vendingTransactionId);
            foundItem = false;
            foreach (var transactionItem in transactionItems)
            {
                if (transactionItem.Id == id)
                {
                    foundItem = true;
                }
            }
            Assert.IsTrue(foundItem);

            // Test get transaction items by year
            transactionItems = _db.GetTransactionItemsForYear(DateTime.UtcNow.Year);
            Assert.IsTrue(transactionItems.Count > 0);
        }
    }
}
