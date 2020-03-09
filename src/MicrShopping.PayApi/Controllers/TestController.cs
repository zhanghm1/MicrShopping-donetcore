using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MicrShopping.PayApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private IConsulClient _consulClient;

        public TestController(ILogger<TestController> logger, IConsulClient consulClient)
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
    }
}
