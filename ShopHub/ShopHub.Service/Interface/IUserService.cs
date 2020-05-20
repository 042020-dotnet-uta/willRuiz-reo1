using ShopHub.Model.DTOs;
using ShopHub.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShopHub.Service.Interface
{
    public interface IUserService
    {
        public Task<User> RegisterUser(UserAuthDto user);
        public Task<UserAuthDto> AuthUser(UserAuthDto user);
    }
}
