using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicrShopping.Domain;
using MicrShopping.Domain.Entities.Orders;
using MicrShopping.Domain.Entities.Products;
using MicrShopping.Infrastructure.Common;
using MicrShopping.OrderApi.Data;
using MicrShopping.OrderApi.Models;
using MicrShopping.OrderApi.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MicrShopping.OrderApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IMapper _mapper;
        private OrderDbContext _orderDbContext;
        private ProductService _productService;

        public ShoppingCartController(OrderDbContext orderDbContext, IMapper mapper, ProductService productService)
        {
            _orderDbContext = orderDbContext;
            _mapper = mapper;
            _productService = productService;
        }

        [HttpGet]
        [Route("List")]
        public async Task<List<ShoppingCartListItemResponse>> ShoppingCartList()
        {
            int userId = UserManage.GetUserId(User);
            List<ShoppingCartListItemResponse> resp = new List<ShoppingCartListItemResponse>();
            var cartList = _orderDbContext.ShoppingCart.Where(a => a.UserId == userId && !a.IsDeleted).ToList();

            //去获取产品列表
            List<ProductListResponse> products = await _productService.GetProductListByIds(string.Join(',', cartList.Select(a => a.ProductId)));

            foreach (var item in cartList)
            {
                var product = products.FirstOrDefault(a => a.Id == item.ProductId);

                ShoppingCartListItemResponse respitem = new ShoppingCartListItemResponse()
                {
                    ProductId = product.Id,
                    ShoppingCartId = item.Id,
                    Number = item.Number,
                    Code = product?.Code,
                    Description = product?.Description,
                    FormerPrice = product?.FormerPrice,
                    Name = product?.Name,
                    RealPrice = product?.RealPrice
                };

                resp.Add(respitem);
            }
            return resp;
        }

        [HttpPost]
        [Route("Add")]
        public async Task<string> AddShoppingCart(AddShoppingCartRequest request)
        {
            int userId = UserManage.GetUserId(User);
            // 准备productList
            List<Product> products = new List<Product>();

            string OrderNo = string.Empty;
            if (_orderDbContext.ShoppingCart.Any(a => a.UserId == userId && a.ProductId == request.ProductId))
            {
                ShoppingCart shoppingCart = _orderDbContext.ShoppingCart.FirstOrDefault(a => a.UserId == userId && a.ProductId == request.ProductId);
                shoppingCart.Number += request.Number;
                _orderDbContext.ShoppingCart.Update(shoppingCart);
            }
            else
            {
                ShoppingCart shoppingCart = new ShoppingCart()
                {
                    UserId = userId,
                    ProductId = request.ProductId,
                    Number = request.Number,
                };
                _orderDbContext.ShoppingCart.Add(shoppingCart);
            }

            _orderDbContext.SaveChanges();

            return OrderNo;
        }
    }
}