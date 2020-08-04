using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MicrShopping.OrderApi.FunctionalTest.Base
{
    public class AutoAuthorizeMiddleware
    {
        private readonly RequestDelegate _next;

        public AutoAuthorizeMiddleware(RequestDelegate rd)
        {
            _next = rd;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var identity = new ClaimsIdentity("cookies");
            identity.AddClaim(new Claim("role", "admin"));
            identity.AddClaim(new Claim("sub", 1.ToString()));
            identity.AddClaim(new Claim("unique_name", "xxxx"));
            identity.AddClaim(new Claim(ClaimTypes.Name, "zzzz"));

            httpContext.User.AddIdentity(identity);

            await _next.Invoke(httpContext);
        }
    }
}