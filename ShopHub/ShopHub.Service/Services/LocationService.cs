using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShopHub.Model.Context;
using ShopHub.Model.DTOs;
using ShopHub.Model.Models;
using ShopHub.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopHub.Service.Services
{
    public class LocationService : ILocation
    {
        /*Dependency Injection Code Start "Scoped".  As we are not creating instance like ProductService product = new ProductService()
         We are creating private types of class type variable (obj) which is populating from constructor. So there is no need
         to dispose the object it will automatically dispose from memory */
        private ShopHubContext _context;
        private IMapper _mapper;
        public LocationService(ShopHubContext shopHubContext, IMapper mapper)
        {
            _context = shopHubContext;
            _mapper = mapper;
        }
        //Dependency Injection Code End

      
        /// <summary>
        /// Get all locations from location table
        /// Uses _context (DbContext) to access DB tables and then apply queries to it 
        /// </summary>
        /// <returns></returns>
        public List<LocationDto> GetAllLocations()
        {
            var locations =  _context.Locations.ToList();   //All location from teh DB
            if (!(locations is null) && locations.Count > 0)
            {
               return _mapper.Map<List<LocationDto>>(locations);    //Convert Model/obj coming from DB
            }                                                       //To DTO as a list of all Locations
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Adds object to context location table
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public LocationDto CreateLocation(LocationDto location)
        {
            var mappedData = _mapper.Map<Location>(location);
            mappedData.Products = new List<Product>();      //We need to initialize this field else we get a null error

            _context.Locations.Add(mappedData); //Pass the converted DTO as Model to the Context
            _context.SaveChanges();             //Save to DB

            return _mapper.Map<LocationDto>(mappedData);    //Converting back to DTO and passing 
        }

        
        /// <summary>
        /// Remove location from database based on ID that is passed to it
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public bool RemoveLocation(int locationId)
        {
            var record = _context.Locations.FirstOrDefault(x => x.Id == locationId);//Uses _context (DbContext) to access DB tables and then apply queries to it
            var response = false;

            if (!(record is null))
            {
                _context.Locations.Remove(record);
                _context.SaveChanges();
                response = true;
                return response;
            }
            else
            {
                return response;
            }
        }

    }
}
