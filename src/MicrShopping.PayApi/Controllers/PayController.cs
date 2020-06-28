using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MicrShopping.Domain.Entities.Pays;
using MicrShopping.Infrastructure.Common;
using MicrShopping.PayApi.Data;
using MicrShopping.PayApi.Models;

namespace MicrShopping.PayApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PayController : ControllerBase
    {
        private readonly ILogger<PayController> _logger;
        private PayDbContext _payDbContext;

        public PayController(ILogger<PayController> logger, PayDbContext payDbContext)
        {
            _logger = logger;
            _payDbContext = payDbContext;
        }
        [HttpGet]
        public string Get()
        {
            return "payapi:" + DateTime.Now;
        }
        [HttpPost]
        public string Post(PayRecoredRequest request)
        {
            if (_payDbContext.PayRecored.Any(a=>a.OrderNo== request.OrderNo))
            {
                throw new ApiExceptionBase("existed", "订单已存在");
            }
            PayRecored payRecored = new PayRecored()
            { 
                UserId= request.UserId,
                OrderNo= request.OrderNo,
                Price= request.Price,
                Status= PayStatus.WaitConfirm
            };

            _payDbContext.PayRecored.Add(payRecored);


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
