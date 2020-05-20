﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ShopHub.Service.Utilities.Enum
{
    public class SessionDetails
    {
        public static string UserId = "UserId";
        public static string UserName = "UserName";
        public static string UserTypeId = "UserTypeId";
    }

    public static class UserTypeNames
    {
        public const string Admin = "1";
        public const string Customer = "2";
    }
}
