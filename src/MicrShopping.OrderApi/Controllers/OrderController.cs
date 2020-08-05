using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Consul;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MicrShopping.Domain;
using MicrShopping.Domain.Entities.Orders;
using MicrShopping.Domain.Entities.Products;
using MicrShopping.Infrastructure.Common;
using MicrShopping.Infrastructure.Common.CapModels;
using MicrShopping.OrderApi.Data;
using MicrShopping.OrderApi.Models;
using MicrShopping.OrderApi.Services;

namespace MicrShopping.OrderApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly ICapPublisher _capBus;
        private readonly IMapper _mapper;
        private OrderDbContext _orderDbContext;
        private ProductService _productService;
        private UserService _userService;
        private IUserManage _userManage;

        public OrderController(ILogger<OrderController> logger
            , ICapPublisher capPublisher
            , OrderDbContext orderDbContext
            , IMapper mapper
            , ProductService productService
            , UserService userService
            , IUserManage userManage
            )
        {
            _logger = logger;
            _capBus = capPublisher;
            _orderDbContext = orderDbContext;
            _mapper = mapper;
            _productService = productService;
            _userService = userService;
            _userManage = userManage;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route("Create")]
        public async Task<string> CreateOrder(CreateOrderForShopingRequest request)
        {
            int userId = _userManage.GetUserId(User);

            var shoppings = _orderDbContext.ShoppingCart.Where(a => a.UserId == userId && request.ShopingIds.Contains(a.Id) && !a.IsDeleted).ToList();
            var productIds = shoppings.Select(a => a.ProductId).ToList();
            // 准备productList
            // webapi接口获取数据
            List<ProductListResponse> products = await _productService.GetProductListByIds(string.Join(',', shoppings.Select(a => a.ProductId)));

            // grpc服务 获取数据
            products = await _productService.GetProductListByIdsGrpc(string.Join(',', shoppings.Select(a => a.ProductId)));

            string accessToken = await HttpContext.GetTokenAsync("access_token");
            //获取用户信息,需要grpc 身份认证，用identityserver4的token
            var userInfo = await _userService.GetUserInfoByIdGrpc(userId, accessToken);

            string OrderNo = string.Empty;
            using (var trans = _orderDbContext.Database.BeginTransaction(_capBus, autoCommit: true))
            {
                Order order = new Order()
                {
                    Address = request.Address,
                    Code = CodePrefix.OrderCodePrefix + Guid.NewGuid().ToString().Replace("-", ""),
                    Status = OrderStatus.WaitPay,
                    UserId = userId
                };
                _orderDbContext.Order.Add(order);

                List<OrderItem> orderItemList = new List<OrderItem>();
                foreach (var item in products)
                {
                    var shopingProducts = shoppings.Where(a => a.ProductId == item.Id);
                    int number = shopingProducts.Sum(a => a.Number);

                    OrderItem orderItem = new OrderItem()
                    {
                        Code = CodePrefix.OrderItemCodePrefix + Guid.NewGuid().ToString().Replace("-", ""),
                        FormerPrice = item.FormerPrice,
                        RealPrice = item.RealPrice,
                        Number = number,
                        ProducName = item.Name,
                        ProductId = item.Id,
                        TotalPrice = item.RealPrice * number,
                        IsDeleted = false,
                        OrderCode = order.Code
                    };
                    orderItemList.Add(orderItem);

                    foreach (var shopingProduct in shopingProducts)
                    {
                        shopingProduct.IsDeleted = true;
                    }
                }
                _orderDbContext.OrderItem.AddRange(orderItemList);

                order.TotalPrice = orderItemList.Sum(a => a.TotalPrice);

                _orderDbContext.SaveChanges();

                ReduceProductModel productModel = new ReduceProductModel()
                {
                    OrderCode = order.Code,
                    ProductItem = orderItemList.Select(a => new ReduceProductItemModel { Id = a.ProductId, Number = a.Number }).ToList()
                };

                _capBus.Publish(CapStatic.ReduceProductCount, productModel);

                OrderNo = order.Code;
                //trans.Commit();
            }

            return OrderNo;
        }

        [Authorize]
        [HttpGet]
        [Route("List")]
        public async Task<PageBase<Order>> OrderList(int PageIndex, int PageSize)
        {
            int userId = _userManage.GetUserId(User);
            var list = _orderDbContext.Order.Where(a => a.UserId == userId && !a.IsDeleted).Skip(PageSize * (PageIndex - 1)).Take(PageSize).ToList();
            PageBase<Order> resp = new PageBase<Order>
            {
                List = list,
                PageIndex = PageIndex,
                PageSize = PageSize,
            };

            return resp;
        }

        [HttpGet]
        [Route("Identity")]
        public IActionResult Identity()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
    }
}