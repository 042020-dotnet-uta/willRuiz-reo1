using AutoMapper;
using ShopHub.Model.DTOs;
using ShopHub.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Automapper help us to map one class of fields to other class fileds, it mapps only the similar fields
/// When field names are teh same, the data is transfered automatically
/// </summary>
namespace ShopHub.Service.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
                    //source , Destination
            CreateMap<User, UserAuthDto>().ReverseMap();
            CreateMap<Location, LocationDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();
        }
    }
}
