using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MicrShopping.OrderApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly ICapPublisher _capBus;
        private IConsulClient _consulClient;

        
        public TestController(ILogger<TestController> logger
            , ICapPublisher capPublisher
            , IConsulClient consulClient
            )
        {
            _logger = logger;
            _capBus = capPublisher;
            _consulClient = consulClient;
        }

        [HttpGet]
        [Route("Get")]
        public async Task<string> Get()
        {
            //using (var consulClient=new ConsulClient(a=>a.Address=new Uri("http://192.168.0.189:8500")))
            //{
            //    var list = await consulClient.Agent.Checks();
                
            //    //var find = list.Response.Where(a => a.Key == "Shoping").Select(a => a.Value);
            //}
                

            _capBus.Publish("xxx.services.show.time", DateTime.Now);
            return "oderapi:"+DateTime.Now;

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
