using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MicrShopping.Domain.Entities.Orders;
using MicrShopping.Domain.Entities.Products;
using MicrShopping.Infrastructure.Common;
using MicrShopping.Infrastructure.Common.CapModels;
using MicrShopping.OrderApi.Data;
using MicrShopping.OrderApi.Models;

namespace MicrShopping.OrderApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly ICapPublisher _capBus;
        private IConsulClient _consulClient;

        private OrderDbContext _orderDbContext;
        public OrderController(ILogger<OrderController> logger
            , ICapPublisher capPublisher
            , IConsulClient consulClient
            , OrderDbContext orderDbContext
            )
        {
            _logger = logger;
            _capBus = capPublisher;
            _consulClient = consulClient;
            _orderDbContext = orderDbContext;
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        [Route("CreateOrder")]
        public async Task<string> CreateOrder(CreateOrderRequest request)
        {
            // 准备productList
            List<Product> products = new List<Product>();
            foreach (var item in request.Data)
            {
                Product product = new Product() {Id=item.ProductId };
                products.Add(product);
            }
            string OrderNo = string.Empty;
            //ResponseBase responseBase = new ResponseBase();
            using (var trans = _orderDbContext.Database.BeginTransaction(_capBus, autoCommit: true))
            {
                Order order = new Order()
                {
                    Address = request.Address,
                    Code = CodePrefix.OrderCodePrefix + Guid.NewGuid().ToString().Replace("-", ""),
                    //TotalPrice = orderItemList.Sum(a => a.TotalPrice),
                    Status = OrderStatus.WaitPay
                };
                _orderDbContext.Order.Add(order);
                

                List<OrderItem> orderItemList = new List<OrderItem>(); 

                foreach (var item in products)
                {
                    int number = request.Data.FirstOrDefault(a => a.ProductId == item.Id).Number;
                    OrderItem orderItem = new OrderItem()
                    {
                        Code = CodePrefix.OrderItemCodePrefix+ Guid.NewGuid().ToString().Replace("-", ""),
                        FormerPrice = item.FormerPrice,
                        RealPrice = item.RealPrice,
                        Number = number,
                        ProducName = item.Name,
                        ProductId = item.Id,
                        TotalPrice = item.RealPrice * number,
                        IsDeleted = false,
                        OrderCode= order.Code
                    };
                    orderItemList.Add(orderItem);
                }
                _orderDbContext.OrderItem.AddRange(orderItemList);

                order.TotalPrice = orderItemList.Sum(a => a.TotalPrice);

                _orderDbContext.SaveChanges();

                ReduceProductModel productModel = new ReduceProductModel() {
                OrderCode=order.Code,
                ProductItem= orderItemList.Select(a => new ReduceProductItemModel { Id = a.ProductId, Number = a.Number }).ToList()
                };

                _capBus.Publish(CapStatic.ReduceProductCount, productModel);

                OrderNo = order.Code;
                //trans.Commit();
            }
            
            return OrderNo;

        }
        [HttpGet]
        [Route("Identity")]
        public IActionResult Identity()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });

        }
        
    }
}
