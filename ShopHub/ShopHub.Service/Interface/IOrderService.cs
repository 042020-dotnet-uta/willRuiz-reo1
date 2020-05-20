﻿using ShopHub.Model.DTOs;
using ShopHub.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopHub.Service.Interface
{
    public interface IOrderService
    {
        public OrderDto SaveOrder(OrderDto order, int actualStockQuantity);
        public List<Order> GetOrderHistoryByUserId(int userId);
        public List<Order> GetAllOrderHistory();
        public List<OrderDto> GetAllStorOrdersByLocationId(int LocationId);

    }
}
