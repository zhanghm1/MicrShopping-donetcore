using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MicrShopping.PayApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PayController : ControllerBase
    {
        private readonly ILogger<PayController> _logger;
        private IConsulClient _consulClient;

        public PayController(ILogger<PayController> logger, IConsulClient consulClient)
        {
            _logger = logger;
            _consulClient = consulClient;
        }
        [HttpGet]
        public string Get()
        {
            return "payapi:" + DateTime.Now;
        }
        [NonAction]
        [CapSubscribe("xxx.services.show.time")]
        public async Task CheckReceivedMessage(DateTime time)
        {
           
            Console.WriteLine(time);

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
