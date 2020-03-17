using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MicrShopping.UserManageApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "111","222","333"};
        }
        [HttpGet("{id}", Name = "Get")]
        [Authorize(Roles = "admin")]
        public string Get(int id)
        {
            return "11111"+ id;
        }
        [HttpGet]
        [Route("Identity")]
        [Authorize]
        public IActionResult Identity()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });

        }
    }
}
