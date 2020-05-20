using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShopHub.Models;
using ShopHub.Service.Interface;

namespace ShopHub.Controllers
{
    public class HomeController : Controller
    {

        private readonly ISessionManager _sessionManager;   //When user logs into app, it is the time during login with credentials
        public HomeController(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        
        

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
