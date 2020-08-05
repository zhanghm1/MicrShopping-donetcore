using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Linq;
using IdentityModel;

namespace MicrShopping.Domain
{
    public class UserManage : IUserManage
    {
        public int GetUserId(ClaimsPrincipal User)
        {
            var claim = User.Claims.Where(a => a.Type == JwtClaimTypes.Subject).FirstOrDefault();
            return Convert.ToInt32(claim.Value);
        }

        public string GetUserName(ClaimsPrincipal User)
        {
            var claim = User.Claims.Where(a => a.Type == JwtClaimTypes.Name).FirstOrDefault();
            return claim.Value;
        }
    }

    public interface IUserManage
    {
        public int GetUserId(ClaimsPrincipal User);

        public string GetUserName(ClaimsPrincipal User);
    }
}