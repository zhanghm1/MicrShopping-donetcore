using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            int UerId = 0;
            List<ShoppingCartListItemResponse> resp = new List<ShoppingCartListItemResponse>();
            var cartList = _orderDbContext.ShoppingCart.Where(a => a.UserId == UerId && !a.IsDeleted).ToList();
            
            //要去获取产品列表
            List<Product> products = new List<Product>();


            foreach (var item in products)
            {
               var respitem = _mapper.Map<ShoppingCartListItemResponse>(item);
                respitem.Number = cartList.FirstOrDefault(a => a.ProdictId == item.Id).Number;
                resp.Add(respitem);
            }
            return resp;
        }

        [HttpPost]
        [Route("Add")]
        public async Task<string> AddShoppingCart(AddShoppingCartRequest request)
        {
            int UerId = 0;
            // 准备productList
            List<Product> products = new List<Product>();
            
            string OrderNo = string.Empty;
            if (_orderDbContext.ShoppingCart.Any(a => a.UserId== UerId && a.ProdictId == request.ProductId))
            {
                ShoppingCart shoppingCart = _orderDbContext.ShoppingCart.FirstOrDefault(a => a.UserId == UerId && a.ProdictId == request.ProductId);
                shoppingCart.Number += request.Number;
                _orderDbContext.ShoppingCart.Update(shoppingCart);
            }
            else
            {
                ShoppingCart shoppingCart = new ShoppingCart();
                _orderDbContext.ShoppingCart.Add(shoppingCart);
            }

            _orderDbContext.SaveChanges();

            return OrderNo;

        }
    }
}