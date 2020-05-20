using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopHub.Filters;
using ShopHub.Model.DTOs;
using ShopHub.Model.Models;
using ShopHub.Service.Interface;
using ShopHub.Service.Utilities.Enum;

namespace ShopHub.Controllers
{
    [AuthFilter(UserTypeNames.Admin)]   //This controller has the rights for just admin
    public class AdminController : Controller
    {
        /*Dependency Injection Code Start we are creating private type of class type variable (obj)
         which is populating from constructor. So there is not need to dispose the object it will automatically dispose from memroy
             */
        private ILocation _location;
        private IProductService _productService;
        private IOrderService _orderService;
        public AdminController(ILocation location, IProductService productService, IOrderService orderService)
        {
            _location = location;
            _productService = productService;
            _orderService = orderService;
        }
        /*Dependency Injection Code End*/


        #region Location i.e Create, Delete , List

        [HttpGet]
        public IActionResult CreateLocation()
        {
            return View();      //Admin / CreatLocation
        }

        /* Post method to create location, each post is checking server side validation that they are ok or not as defined on annoations.
           If model data is valid -> CreateLocation service to create location   */
        [HttpPost]
        public IActionResult CreateLocation(LocationDto location)
        {
            if (ModelState.IsValid)
            {
                _location.CreateLocation(location);     //adds data to the DB
                return RedirectToAction("Location");    //  Overloaded this controller to the Locatation()
            }                                           //Admin / Location -> View
            else
            {
                return View(location);
            }
        }
        /*Delete the location by its Id ->after remove location -> RedirectToAction redirect page to location listing
         page where we can see location is remove or not. */
        public IActionResult DeleteLocation(int locationId)
        {
            _location.RemoveLocation(locationId);
            return RedirectToAction("Location");
        }

        /*GetAllLocations, for listing purpose  */
        public IActionResult Location()     
        {
            var locations = _location.GetAllLocations();    //Returns a list of all available Locations (as DTO)
            if (locations is null)
            {
                locations = new List<LocationDto>();
            }
            return View(locations);     //Givers this List to the view ... Admin / Location
        }
        #endregion


        #region Products All CRUD

        /* GET method for creating product -> use GetAllLocations service to populate locations dropdown for this new product*/

        [HttpGet]
        public IActionResult CreateProduct()
        {
            ProductDto product = new ProductDto();
            var locations = _location.GetAllLocations();    //List of all Locations as DTO
            if (locations is null)
            {
                locations = new List<LocationDto>();
            }
            else
            {
                product.Locations = locations;      //Assigning Locations to a potentially new Product
            }                                       //Product needs to be filled out in the form but, now it has a drop-down
            return View(product);                   //Liost of locations to choose from
        }

        /* POST method to create product -> Each POST checking server side validation that they are ok or not as defined on annoations.
           If model data is valid -> AddProduct service to create product */
        [HttpPost]
        public IActionResult CreateProduct(ProductDto product)
        {
            if (ModelState.IsValid)
            {
                _productService.AddProduct(product);        //Add the entry from the form mode<-> DTo see method .. save to DB
                return RedirectToAction("ProductList");     //Go to Admin / ProductList -> View   
            }
            else
            {
                return View(product);
            }
        }

        /*Display all products for all locations  */
        public IActionResult ProductList()
        {
            var products = _productService.GetAllProducts();
            if (products is null)
            {
                products = new List<ProductDto>();
            }
            return View(products);  //Admin / ProductList ... passes all products to this View
        }

        /*Delete product by Id -> RedirectToAction to "productList" page where we can see product if remove / not */
        public IActionResult DeleteProduct(int productId)
        {
            _productService.RemoveProduct(productId);
            return RedirectToAction("ProductList");
        }

        /*GET method to update product -> as this is the get method so it will populate relevant data to its fields.
          Get the product by Id -> populate fields for update */
        [HttpGet]
        public IActionResult UpdateProduct(int productId)
        {
           var productData = _productService.GetProductById(productId);
            var locations = _location.GetAllLocations();
            if (locations is null)
            {
                locations = new List<LocationDto>();
                productData.Locations = locations;
            }
            else
            {
                productData.Locations = locations;
            }
           return View(productData);
        }

        /*POST  method to UpdateProduct service -> redirect to page, see the changes reflected
             */
        [HttpPost]
        public IActionResult UpdateProduct(ProductDto product)
        {
            if (ModelState.IsValid)
            {
                _productService.UpdateProduct(product);
                return RedirectToAction("ProductList");
            }
            else
            {
                return View(product);
            }
        }
        #endregion

        #region Customer Order History Details

        //This method is based on Location to populate the locations dropdown. Location history will populate with another method by making an ajax get call
        public IActionResult LocationOrderHistory()
        {
            OrderDto product = new OrderDto();

            var locations = _location.GetAllLocations();    //Get all Locations from the DB
            if (locations is null)
            {
                locations = new List<LocationDto>();
                product.Locations = locations;
            }
            else
            {
                product.Locations = locations;      //These are the locations tied to Products
            }
            return View(product);
        }

        // Populate orders base on location Id
        // Helper method to LocationOrderHistory
        // Called through ajax get request from LocationOrderHistory View
        public IActionResult LocationBaseOrderData(int locationId)  //Get all orders based on Location ID
        {
           var data = _orderService.GetAllStorOrdersByLocationId(locationId);
           return Json(data);   //This ID is passed to this controller ... calls the DB and .. 
            //returns info to page as asynch w/o hitting the Server ... Basically updating the Page to show
            // All listed Products for a single location ...Returs only Dta NOT the View
            //This happens on Admin / LocationOrderHistory
        }

        // In this method all order of stores are populating without any location specify
        public IActionResult AllOrderHistory()
        {
            var orderDetails = _orderService.GetAllOrderHistory();//List of all users+ products + locations etc..
            if (!(orderDetails is null) && orderDetails.Count > 0)
            {
                return View(orderDetails);  // Admin / AllOrderHistory + pass the var
            }
            else
            {
                orderDetails = new List<Order>();
                return View(orderDetails);
            }
        }
        #endregion
    }
}