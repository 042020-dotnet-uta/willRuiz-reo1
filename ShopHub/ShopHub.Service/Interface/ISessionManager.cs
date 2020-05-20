using System;
using System.Collections.Generic;
using System.Text;

namespace ShopHub.Service.Interface
{
    /// <summary>
    /// These are teh classes that must be defined for how a Sessoin will work in teh application
    /// </summary>
    public interface ISessionManager
    {
        public void SetUserId(int userId);      
        public void SetUserName(string userName);
        public void SetUserTypeId(int userTypeId);
        public int GetUserId();
        public string GetUserName();
        public int GetUserTypeId();
        public void SessionClear();
    }
}
