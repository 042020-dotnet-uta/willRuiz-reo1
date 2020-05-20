using Microsoft.AspNetCore.Http;
using ShopHub.Service.Interface;
using ShopHub.Service.Utilities.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopHub.Service.Services
{
    public class SessionManager: ISessionManager
    {
        //IHttpContextAccessor give us access to the browser sessions
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISession _session;

        public SessionManager(IHttpContextAccessor httpContextAccessor)
        {
            _session = httpContextAccessor.HttpContext.Session;
        }

        public int GetUserId()
        {
            return Convert.ToInt32(_session.GetString(SessionDetails.UserId));
        }

        public string GetUserName()
        {
            return _session.GetString(SessionDetails.UserName);
        }

        //Get login UserTypeId
        public int GetUserTypeId()
        {
            return Convert.ToInt32(_session.GetString(SessionDetails.UserTypeId));
        }

        public void SessionClear()
        {
            _session.Clear();
        }

        public void SetUserId(int userId)
        {
            _session.SetString(SessionDetails.UserId, userId.ToString());
        }

        //Set login UserName
        public void SetUserName(string userName)
        {
            _session.SetString(SessionDetails.UserName, userName);
        }

        //Set login UserTypeId
        public void SetUserTypeId(int userTypeId)
        {
            _session.SetString(SessionDetails.UserTypeId, userTypeId.ToString());
        }
    }
}
