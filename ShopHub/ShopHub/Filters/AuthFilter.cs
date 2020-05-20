using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using ShopHub.Service.Utilities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopHub.Filters
{
    /* Asp.net Mvc - five types filters which runs
     1) before and 2) after the request came to specific method or controllers.
     3) "ActionFilterAttribute" method = OnActionExecuting
     AuthFilter in this Controller is filtering out users
     by userTypes aka role type.

     User authentication + authorization filter.
     It checks wheather sessions of user are available or not, 
     If sessions are NOT available redirect -> user to login page.
     If user != admin then user cannot visit the admin page
        */
    public class AuthFilter : ActionFilterAttribute
    {
        public string[] Roles { get; set; }
        public AuthFilter(params string[] roles)
        {
            this.Roles = roles;
        }
        /// <summary>
        /// Calls before action executes
        /// Chexks if a session of a user is available or not
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {                                                                       //(1=Admin, 2=Customer)
            var userTypeId = context.HttpContext.Session.GetString(SessionDetails.UserTypeId);  //Which type of user is logged in

            if (context.HttpContext.Session.GetString(SessionDetails.UserId) == null)   //Session exists /not
            {
                string[] excludePath = { "/AuthUser/Login" };//If does not exist, back to the controller

                if (!excludePath.Contains(context.HttpContext.Request.Path.Value))  //Current request url
                {
                    bool isAjaxCall = context.HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest";

                    if (!isAjaxCall)
                    {
                        context.Result = new RedirectToRouteResult("Default",   //No user sessions go to login page
                           new RouteValueDictionary{
                                {"controller", "AuthUser"},
                                {"action", "Login"},
                                {"returnUrl", context.HttpContext.Request.Path.Value}
                           });
                    }
                }
            }
            else
            {
                if (!Roles.Contains(userTypeId))    //Session exists.  Check the Role(1/2), Does User type exist (Cust/Admin)
                {
                    context.Result = new RedirectToRouteResult("Default",
                            new RouteValueDictionary{
                            {"controller", "Home"},
                            {"action", "AccessDenied"}, //Customer trying to access admin
                            {"returnUrl", context.HttpContext.Request.Path.Value}
                            });
                }

            }

            base.OnActionExecuting(context);
        }

    }
}
