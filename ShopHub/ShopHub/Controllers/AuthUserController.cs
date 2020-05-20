 using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopHub.Model.DTOs;
using ShopHub.Model.Models;
using ShopHub.Service.Interface;
using ShopHub.Service.Services;

namespace ShopHub.Controllers
{
    public class AuthUserController : Controller    //How we login / log out ... it auth the user
    {
        private ISessionManager _sessionManager;
        private IUserService _userService;          //Login logout register for a user
        public AuthUserController(ISessionManager sessionManager, IUserService userService)
        {
            _sessionManager = sessionManager;
            _userService = userService;
        }
        
        //Register new user to our system
        [HttpGet]
        public IActionResult Register()
        {
            return View();      //View AuthUser Register
        }

        [HttpPost]      //Form with credentials
        public async Task<IActionResult> Register(UserAuthDto userModel) //Obj from for is bound and passed to this Controller method
        {
            if (ModelState.IsValid) //Has all creds required ... against our validation in DTO
            {
              var result = await _userService.RegisterUser(userModel); //DTO converted and all new registration, saved to DB

                _sessionManager.SetUserId(result.Id);   //Getting data to set the session 
                _sessionManager.SetUserName(result.FirstName + " " + result.LastName);
                _sessionManager.SetUserTypeId(result.UserTypeId);   //Session is now complete w/ the user data
                return RedirectToAction("Index","Home");    //RedirectToASction("Method", "Controller")
            }
            else
            {
                return View(userModel);
            }
        }
        public IActionResult Login()    //Login w/ existing user credentials
        {
            return View();  //AuthUser Login View
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserAuthDto userModel)   //userModel passed from View
        {
            if (ModelState.IsValid) 
            {
                var result = await _userService.AuthUser(userModel);    //validation for successful login ...DTO returned
                if (result.IsSuccessFullLogin)
                {
                    _sessionManager.SetUserId(result.Id);
                    _sessionManager.SetUserName(result.FirstName + " " + result.LastName);
                    _sessionManager.SetUserTypeId(result.UserTypeId);   //Session is alive by setting the details
                    return RedirectToAction("Index", "Home");   //RedirectToAction("Method", "Controller")
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong with your login details.");
                    return View(userModel); //AuthUser Login asp-validation-summary ... pass string message
                }
            }
            else
            {
                return View(userModel);
            }

        }

        public IActionResult Logout()
        {
            _sessionManager.SessionClear();     //Kills the session
            return RedirectToAction("Login");   //Back to login Page ...AuthUser / Login
        }

    }
}