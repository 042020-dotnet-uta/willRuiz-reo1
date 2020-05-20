using AutoMapper;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ShopHub.Model.Context;
using ShopHub.Model.DTOs;
using ShopHub.Model.Models;
using ShopHub.Service.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ShopHub.Service.Services
{
    public class OrderService : IOrderService
    {
        private ShopHubContext _context;
        private IMapper _mapper;
        private IProductService _productService;
        public OrderService(ShopHubContext shopHubContext, IMapper mapper, IProductService productService)
        {
            _context = shopHubContext;
            _mapper = mapper;
            _productService = productService;
        }

       
        /// <summary>
        /// Save order of customer to database + reduce quantity of ordered product from our stock
        /// </summary>
        /// <param name="order"></param>
        /// <param name="actualStockQuantity"></param>
        /// <returns></returns>
        public OrderDto SaveOrder(OrderDto order, int actualStockQuantity)
        {
            var mappedData = _mapper.Map<Order>(order);
            mappedData.Product = null;

            _context.Orders.Add(mappedData);
            _context.SaveChanges();

            var currentQuantity = actualStockQuantity - order.Quantity;
            _productService.MinusProductQuantity(order.ProductId, currentQuantity);

            return new OrderDto()
            {
                IsSucceedOrder = true
            };

        }

       
        /// <summary>
        /// Get order Details by specific userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Order> GetOrderHistoryByUserId(int userId)
        {
            //Include(x => x.Product) which means to join and get order data with product details. 
            //ThenInclude(...) Similar case for the location and User Include
   
              var orderDetails =  _context.Orders.Include(x => x.Product).ThenInclude(x=>x.Location).Include(x => x.User).Where(x => x.UserId == userId).OrderByDescending(x=>x.Id).ToList();
            if (!(orderDetails is null) && orderDetails.Count > 0)
            {
                return orderDetails;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets all order details without specifying userId
        /// </summary>
        /// <returns></returns>
        public List<Order> GetAllOrderHistory()
        {
                          //All orders from DB + Join Products + Join Locations + Join Users + Display by Id high low + as List
            var orderDetails = _context.Orders.Include(x => x.Product).ThenInclude(x => x.Location).Include(x => x.User).OrderByDescending(x => x.Id).ToList();
            if (!(orderDetails is null) && orderDetails.Count > 0)
            {
                return orderDetails;
            }
            else
            {
                return null;
            }
        }

        
        /// <summary>
        /// Get all orders of specific location store
        /// </summary>
        /// <param name="LocationId"></param>
        /// <returns></returns>
        public List<OrderDto> GetAllStorOrdersByLocationId(int LocationId) 
        { 
            using (var conn = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            { 

                var result =  conn.Query<OrderDto>(sql: "[spGetOrdersByLocation]", param: new { locationId = LocationId },
                commandType: CommandType.StoredProcedure); 
                
                return result.ToList(); 
            
            }
            /*
          This method is differnt from all of others , as in this method data is fetching from multiple tables of database using "SQL Stored Procedure"
          spGetOrdersByLocation store procedure to get all orders against specific location store.
          We can connect directly to SQL Stored Procedure using Dapper ORM (Object Relationship Mapper) */

        }

    }
}
