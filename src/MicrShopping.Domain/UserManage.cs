using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Linq;

namespace MicrShopping.Domain
{
    public static class UserManage
    {
        public static int GetUserId(ClaimsPrincipal User)
        {
            var claim = User.Claims.Where(a => a.Type == "sub").FirstOrDefault();
            return Convert.ToInt32(claim.Value);
        }
        public static int GetUserName(ClaimsPrincipal User)
        {
            var claim = User.Claims.Where(a => a.Type == "name").FirstOrDefault();
            return Convert.ToInt32(claim.Value);
        }
    }
}
