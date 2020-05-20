using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShopHub.Model.Context;
using ShopHub.Model.DTOs;
using ShopHub.Model.Models;
using ShopHub.Service.Interface;
using ShopHub.Service.Utilities.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShopHub.Service.Services
{
    public class UserService : IUserService
    {
        private ShopHubContext _context;
        private IMapper _mapper;
        public UserService(ShopHubContext shopHubContext, IMapper mapper)
        {
            _context = shopHubContext;
            _mapper = mapper;
        }

        //This method is use to register user to our database
        public async Task<User> RegisterUser(UserAuthDto user)  //We accept a DTO
        {
            user.UserTypeId = Convert.ToInt32(UserTypeNames.Customer);
            var mappedData = _mapper.Map<User>(user);   //Convert from DTO to User Model -> this var has as a Model
            _context.Users.Add(mappedData);             //Add to DB
            await _context.SaveChangesAsync();          //Save to the DB

            return mappedData;
        }

        /*Authenticate user to our database, like 
         FirstName, LastName, Password matches some record -or- not*/
        public async Task<UserAuthDto> AuthUser(UserAuthDto user)
        {
            //_context.Users is checking against the DB ... validating against user.Attribute that was passed
            var record = await _context.Users.FirstOrDefaultAsync(x => (x.FirstName.ToLower().Equals(user.FirstName)) 
                           && x.LastName.ToLower().Equals(user.LastName) && x.Password.ToLower().Equals(user.Password));

            if (!(record is null))  //login sucess
            {
                var mappedData = _mapper.Map<UserAuthDto>(record);
                mappedData.IsSuccessFullLogin = true;   //set to true b/c above is validated
                return mappedData;  //Return the DTO
            }
            else
            {
                var mappedData = _mapper.Map<UserAuthDto>(user);
                mappedData.IsSuccessFullLogin = false;
                return mappedData;
            }
        }

    }
}
