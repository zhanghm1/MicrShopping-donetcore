﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicrShopping.Domain;
using MicrShopping.Domain.Entities.Orders;
using MicrShopping.Domain.Entities.Products;
using MicrShopping.OrderApi.Data;
using MicrShopping.OrderApi.Models;

namespace MicrShopping.OrderApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IMapper _mapper;
        private OrderDbContext _orderDbContext;
        public ShoppingCartController(OrderDbContext orderDbContext, IMapper mapper)
        {
            _orderDbContext = orderDbContext;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("List")]
        public async Task<List<ShoppingCartListItemResponse>> ShoppingCartList()
        {
            int userId = UserManage.GetUserId(User);
            List<ShoppingCartListItemResponse> resp = new List<ShoppingCartListItemResponse>();
            var cartList = _orderDbContext.ShoppingCart.Where(a => a.UserId == userId && !a.IsDeleted).ToList();
            
            //要去获取产品列表
            List<Product> products = new List<Product>();


            foreach (var item in cartList)
            {
                var product = products.FirstOrDefault(a => a.Id == item.ProdictId);

                ShoppingCartListItemResponse respitem = new ShoppingCartListItemResponse()
                { 
                
                ProductId=1,
                ShoppingCartId= item.Id,
                Number=item.Number,
                Code= product?.Code,
                Description= product?.Description,
                FormerPrice= product?.FormerPrice,
                Name= product?.Name,
                RealPrice= product?.RealPrice
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
            if (_orderDbContext.ShoppingCart.Any(a => a.UserId== userId && a.ProdictId == request.ProductId))
            {
                ShoppingCart shoppingCart = _orderDbContext.ShoppingCart.FirstOrDefault(a => a.UserId == userId && a.ProdictId == request.ProductId);
                shoppingCart.Number += request.Number;
                _orderDbContext.ShoppingCart.Update(shoppingCart);
            }
            else
            {
                ShoppingCart shoppingCart = new ShoppingCart()
                { 
                UserId= userId,
                ProdictId= request.ProductId,
                Number= request.Number,
                };
                _orderDbContext.ShoppingCart.Add(shoppingCart);
            }

            _orderDbContext.SaveChanges();

            return OrderNo;

        }
    }
}