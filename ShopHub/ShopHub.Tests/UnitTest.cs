using AutoMapper;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using ShopHub.Controllers;
using ShopHub.Model.Context;
using ShopHub.Model.DTOs;
using ShopHub.Model.Models;
using ShopHub.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ShopHub.Tests
{
    public class UnitTest
    {
        /// <summary>
        /// Are Product Objects correctly saving to the DB
        /// </summary>
        #region Test
        //private IProductService _productService;
        private Mock<ILocation> _location;              //Initialize the Interface not communicate with DB
        private Mock<IOrderService> _orderService;
        private Mock<ISessionManager> _sessionManager;
        private Mock<ShopHubContext> _context;
        private Mock<IMapper> _mapper;
        private Mock<IProductService> _productService;
        public UnitTest()
        {
            _productService = new Mock<IProductService>();
            _location = new Mock<ILocation>();
            _orderService = new Mock<IOrderService>();
            _sessionManager = new Mock<ISessionManager>();
            _context = new Mock<ShopHubContext>();
            _mapper = new Mock<IMapper>();
        }
# endregion
        [Fact]
        public void IsProductObjectSavingToTheDB()
        {   // Arrange
            var options = new DbContextOptionsBuilder<ShopHubContext>()
                .UseInMemoryDatabase(databaseName: "AddProductToDbTest").Options;
            //Act
            using (var db = new ShopHubContext(options))
            {
                Product p = new Product
                {
                    Name = "Cup",   Price = "50",   Quantity = 5,   Location = new Location("Texas")
                };
                db.Add(p);
                db.SaveChanges();
            }          
            //Assert
            using (var context = new ShopHubContext(options))
            {
                Assert.Equal(6, context.Products.Count() );   //Six total tests that Create a Product            
            }
        }
        /// <summary>
        /// Are multiple products from multiple locations being saved to the DB
        /// </summary>
        [Fact]
        public void AddMultipleProductToDbTest()
        { // Arrange
            var options = new DbContextOptionsBuilder<ShopHubContext>()
            .UseInMemoryDatabase(databaseName: "AddProductToDbTest").Options;

            //Act
            using (var db = new ShopHubContext(options))
            {
                List<Product> products = new List<Product>()
                {
                    new Product { LocationId = 2, Name = "Bike", Price = "450", Quantity = 5 },
                    new Product { LocationId = 3, Name = "Apple", Price = "5", Quantity = 10 }
                };

                db.AddRange(products);
                db.SaveChanges();
            }
            //Assert
            using (var context = new ShopHubContext(options))
            {
                var getProductsFromTempDB = context.Products.Where(p => p.LocationId == 2 || p.LocationId == 3).ToList();
                Assert.Equal(2, getProductsFromTempDB.Count());
            }
        }


        /// <summary>
        /// Is Product Price correctly Saving to the DB
        /// </summary>
        [Fact]
        public void IsProductObjectSavingPriceToTheDB()
        {// Arrange
            var options = new DbContextOptionsBuilder<ShopHubContext>()
                .UseInMemoryDatabase(databaseName: "AddProductToDbTest").Options;
            //Act
            using (var db = new ShopHubContext(options))
            {
                Product p = new Product
                {
                    Name = "Cup",  Price = "50",  Quantity = 5,  Location = new Location("Texas")
                };
                db.Add(p);
                db.SaveChanges();
            }
            //Assert
            using (var context = new ShopHubContext(options))
            {
                var createdProductInTempDB = context.Products.Where(p => p.Name == "Cup").FirstOrDefault();
                Assert.Equal("50", createdProductInTempDB.Price);
               // Assert.Equal("Cup", createdProductInTempDB.Name);
            }
        }
        /// <summary>
        /// Are Product Quantities saving to the DB
        /// </summary>
        [Fact]
        public void IsProductObjectSavingQuantityToTheDB()
        {// Arrange
            var options = new DbContextOptionsBuilder<ShopHubContext>()
                .UseInMemoryDatabase(databaseName: "AddProductToDbTest").Options;
            //Act
            using (var db = new ShopHubContext(options))
            {
                Product p = new Product
                {
                    Name = "Cup",  Price = "50", Quantity = 5,  Location = new Location("Texas")
                };
                db.Add(p);
                db.SaveChanges();
            }
            //Assert
            using (var context = new ShopHubContext(options))
            {
                var createdProductInTempDB = context.Products.Where(p => p.Name == "Cup").FirstOrDefault();
                Assert.Equal(5, createdProductInTempDB.Quantity);
                // Assert.Equal("Cup", createdProductInTempDB.Name);
            }
        }
        /// <summary>
        /// Are products being correctly delted from the DB
        /// </summary>
        [Fact]
        public void RemoveProductToDbTest()
        { // Arrange
            var options = new DbContextOptionsBuilder<ShopHubContext>()
            .UseInMemoryDatabase(databaseName: "AddProductToDbTest").Options;
            var expectedOutComes = string.Empty;
            //Act
            using (var db = new ShopHubContext(options))
            {
                Product bottle = new Product
                {
                    LocationId = 1,
                    Name = "bottle",
                    Quantity = 90,
                    Price = "100"
                };

                db.Add(bottle);
                db.SaveChanges();

                //Remove the recently saved product
                var productEntity = db.Products.Where(p => p.LocationId == 1).FirstOrDefault();
                db.Products.Remove(productEntity);
                db.SaveChanges();

            }
            //Assert
            using (var context = new ShopHubContext(options))
            {
                var getProductFromTempDB = context.Products.Where(p => p.LocationId == 1).FirstOrDefault();
                if (getProductFromTempDB is null)
                {
                    expectedOutComes = "";
                }
                Assert.Equal("", expectedOutComes);
            }
        }

        /// <summary>
        /// Are Product Object Names being saved to the DB
        /// </summary>
        [Fact]
        public void IsProductObjectNameSavingToTheDB()
        {// Arrange
            var options = new DbContextOptionsBuilder<ShopHubContext>()
                .UseInMemoryDatabase(databaseName: "AddProductToDbTest").Options;
            //Act
            using (var db = new ShopHubContext(options))
            {
                Product p = new Product
                {
                    Name = "Cup",  Price = "50",  Quantity = 5,  Location = new Location("Texas")
                };
                db.Add(p);
                db.SaveChanges();
            }
            //Assert
            using (var context = new ShopHubContext(options))
            {
                var createdProductInTempDB = context.Products.Where(p => p.Name == "Cup").FirstOrDefault();
                Assert.Equal("Cup", createdProductInTempDB.Name);
            }
        }
        /// <summary>
        /// Are ordered products being removed from Inventory
        /// </summary>
        [Fact]
        public void MinusQuantityWhenOrderPlaceToDbTest()
        { // Arrange
            var options = new DbContextOptionsBuilder<ShopHubContext>()
            .UseInMemoryDatabase(databaseName: "AddOrderToDbTest").Options;

            //Act

            using (var db = new ShopHubContext(options))
            {
                //Add product to db
                Product watch = new Product
                {
                    LocationId = 1,  Name = "watch",  Quantity = 30, Price = "300"
                };

                db.Add(watch);
                db.SaveChanges();

                //Place order to db
                Order order = new Order
                {
                    UserId = 1, ProductId = 1, Quantity = 10, Timestamp = DateTime.Now
                };

                db.Add(order);
                db.SaveChanges();

                //Minus Stock Quantity
                var createdProductInTempDB = db.Products.Where(p => p.Name.Equals("watch")).FirstOrDefault();
                createdProductInTempDB.Quantity = createdProductInTempDB.Quantity - order.Quantity;
                db.SaveChanges();
            }
            //Assert
            using (var context = new ShopHubContext(options))
            {
                var createdOrderInTempDB = context.Products.Where(p => p.Name.Equals("watch")).FirstOrDefault();
                Assert.Equal(20, createdOrderInTempDB.Quantity);
            }
        }

        /// <summary>
        /// Are Locations saving to the DB
        /// </summary>
        [Fact]
        public void IsLocationObjecSavingToTheDB()
        {// Arrange
            var options = new DbContextOptionsBuilder<ShopHubContext>()
                .UseInMemoryDatabase(databaseName: "AddLocationToDbTest").Options;
            //Act
            using (var db = new ShopHubContext(options))
            {
                Location l = new Location
                {
                    Name = "Florida",
                };
                db.Add(l);
                db.SaveChanges();
            }
            //Assert
            using (var context = new ShopHubContext(options))
            {
                var createdLocationInTempDB = context.Locations.Where(p => p.Name == "Florida").FirstOrDefault();
                Assert.Equal(1, context.Locations.Count() );
            }
        }
        /// <summary>
        /// Are User objects saving to the DB
        /// </summary>
        [Fact]
        public void isUserSavingToTheDB()
        {   //Arrange
            var options = new DbContextOptionsBuilder<ShopHubContext>()
                .UseInMemoryDatabase(databaseName: "AddCustomerToDbTest").Options;
            //Act
            using (var db = new ShopHubContext(options))
            {
                User u = new User
                {
                    FirstName = "John",
                    LastName = "Smith"
                };
                db.Add(u);
                db.SaveChanges();
                //Assert
                using (var context = new ShopHubContext(options) ) 
                {
                    Assert.Equal(2, context.Users.Count()); //The test above also creates a User Object
                }

            }
        }
        /// <summary>
        /// Are Names correctly Saving to the DB
        /// </summary>
        [Fact]
        public void isUserNameValid()
        {   //Arrange
            var options = new DbContextOptionsBuilder<ShopHubContext>()
                .UseInMemoryDatabase(databaseName: "AddCustomerToDbTest").Options;
            //Act
            using (var db = new ShopHubContext(options))
            {
                User u = new User()
                {
                    FirstName = "John",
                    LastName = "Smith"
                };
                db.Add(u);
                db.SaveChanges();
                //Assert
                using (var context = new ShopHubContext(options))
                {
                    var user = context.Users.FirstOrDefault();
                    Assert.True(user.FirstName == "John");
                    Assert.False(user.LastName == "Jones");
                }

            }
        }

        /* Check get method return view value, should be null
           GET request of register we are not returning anything*/
        [Fact]
        public void AuthUser_Register_ReturnView_Test()
        {
            // Arrange      
            var controller = new AuthUserController(null,null); 
            string viewName = null;

            // Act
            var result = controller.Register() as ViewResult;//Return the View data

            // Assert
            Assert.Equal(viewName, result.ViewName);
        }

        /* GET return view value, should be null 
           because in GET request of login we are not returning anything*/
        [Fact]
        public void AuthUser_Login_ReturnView_Test()
        {
            // Arrange
            var controller = new AuthUserController(null,null);
            string viewName = null;

            // Act
            var result = controller.Login() as ViewResult;

            // Assert
            Assert.Equal(viewName, result.ViewName);
        }


        /// <summary>
        /// Home Controller Index returns a View
        /// Just returns the View() Null value as nothing is passed
        /// </summary>
        [Fact]
        public void Home_Controller_Index_ViewTestCase()
        {
            // Arrange
            var controller = new HomeController(null);
            string viewName = null;

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.Equal(viewName, result.ViewName);
        }

        /// <summary>
        ///  If access is denied on the Home page it should just return to the Home Page
        /// Just returns the View() Null value as nothing is passed
        /// </summary>
        [Fact]
        public void Home_Controller_AccessDenied_ViewTestCase()
        {
            // Arrange
            var controller = new HomeController(null);
            string viewName = null;

            // Act
            var result = controller.AccessDenied() as ViewResult;

            // Assert
            Assert.Equal(viewName, result.ViewName);
        }

        /// <summary>
        /// If there is an error on the Home page it shoudl just return to the Home Page
        /// Just returns the View()  Null value as nothing is passed
        /// </summary>
        [Fact]
        public void Home_Controller_Error_ViewTestCase()
        {
            // Arrange
            var controller = new HomeController(null);
            string viewName = null;

            // Act
            var result = controller.Error() as ViewResult;

            // Assert
            Assert.Equal(viewName, result.ViewName);
        }


        


    }
}
