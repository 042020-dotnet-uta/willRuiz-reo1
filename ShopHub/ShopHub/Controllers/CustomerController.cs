using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopHub.Filters;
using ShopHub.Model.DTOs;
using ShopHub.Model.Models;
using ShopHub.Service.Interface;
using ShopHub.Service.Utilities.Enum;

namespace ShopHub.Views.Home
{
    [AuthFilter(UserTypeNames.Admin,UserTypeNames.Customer)] // Admin/Cust both filtered to see what type of login user (admin/cust)
    public class CustomerController : Controller    //This controller has the rights for both the admin and customer
    {
        private IProductService _productService;
        private ILocation _location;
        private IOrderService _orderService;
        private ISessionManager _sessionManager;
        public CustomerController(IProductService productService, ILocation location, IOrderService orderService, ISessionManager sessionManager)
        {
            _productService = productService;
            _location = location;
            _orderService = orderService;
            _sessionManager = sessionManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        //Populate location dropdown on store page
        public IActionResult StorePlace()
        {
            ProductDto product = new ProductDto();

            var locations = _location.GetAllLocations();
            if (locations is null)
            {
                locations = new List<LocationDto>();
                product.Locations = locations;
            }
            else
            {
                product.Locations = locations;
            }
            return View(product);       //Customer/ Store Place
        }

        /*Method will render all products of selected location.  This method return json array of products to view and we are populating these arrays to our view in tables*/
        public IActionResult GetProductsAgainstLocation(int locationId)
        {
            var products = _productService.GetProductsByLocationId(locationId); //Called in the view as a JS functoin
            if (!(products is null))
            {
                return Json(products);
            }
            else
            {
                products = new List<ProductDto>();
                return Json(products);
            }
        }

        //StorPlace View
        public IActionResult PlaceOrder(int userId, int productId, int quantity, int actualStockQuantity)
        {
            OrderDto order = new OrderDto()
            {
                UserId = userId,
                ProductId = productId,
                Quantity = quantity,
                ActualStockQuantity = actualStockQuantity,
                Timestamp = DateTime.Now
                //DateTime.UtcNow
            };
             _orderService.SaveOrder(order, actualStockQuantity);
                                //True returs PNotify success else see error tree
            var returnobj = new { IsError = false, Message = "Your order is successfully placed" };
            return Json(returnobj); //This is being passed to View Customer / Store Place
        }

        //Customer can view their order history using this method
        public IActionResult OrderHistory()
        {
            var userId = _sessionManager.GetUserId();                           
            var orderDetails = _orderService.GetOrderHistoryByUserId(userId);   //Get order history of user by Id
            if (!(orderDetails is null) && orderDetails.Count > 0)
            {
                return View(orderDetails);
            }
            else
            {
                orderDetails = new List<Order>();
                return View(orderDetails);
            }
        }
    }
}