using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;
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

        public TestController(ILogger<TestController> logger
            , ICapPublisher capPublisher
            )
        {
            _logger = logger;
            _capBus = capPublisher;
        }
        [HttpGet]
        [Route("Health")]
        public IActionResult Health()
        {
            return Content("OK");
        }

        [HttpGet]
        public string Get()
        {

            //_capBus.Publish("xxx.services.show.time", DateTime.Now);
            return "oderapi:"+DateTime.Now;

        }
    }
}
