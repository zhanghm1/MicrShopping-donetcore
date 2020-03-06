using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
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
