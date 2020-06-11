using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Linq;
using IdentityModel;

namespace MicrShopping.Domain
{
    public static class UserManage
    {

        public static int GetUserId(ClaimsPrincipal User)
        {
            var claim = User.Claims.Where(a => a.Type == JwtClaimTypes.Subject).FirstOrDefault();
            return Convert.ToInt32(claim.Value);
        }
        public static string GetUserName(ClaimsPrincipal User)
        {
            var claim = User.Claims.Where(a => a.Type == JwtClaimTypes.Name).FirstOrDefault();
            return claim.Value;
        }
    }
}
