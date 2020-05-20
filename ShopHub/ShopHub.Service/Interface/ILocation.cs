using ShopHub.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShopHub.Service.Interface
{
    public interface ILocation
    {
        public LocationDto CreateLocation(LocationDto location);
        public List<LocationDto> GetAllLocations();
        public bool RemoveLocation(int locationId);
    }
}
