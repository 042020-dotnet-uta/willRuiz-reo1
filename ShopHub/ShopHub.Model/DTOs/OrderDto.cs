using System;
using System.Collections.Generic;
using System.Text;

namespace ShopHub.Model.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime Timestamp { get; set; }
        public string StrTimestamp { get; set; }    //Used to format the Date as a string
        public bool IsSucceedOrder { get; set; }    //Was this saved to DB T/F
        public int ActualStockQuantity { get; set; }    //Return the actual quantity held at the store from DB
        public List<LocationDto> Locations { get; set; }   //Used for multiple locations of drop down
        public int LocationId { get; set; }
        public string ProductName { get; set; }
        public string CustomerName { get; set; }
        public int Price { get; set; }
    }
}
