using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using IdentityModel;
using Microsoft.EntityFrameworkCore.Internal;
using MicrShopping.Domain;
using MicrShopping.OrderApi.Controllers;
using MicrShopping.OrderApi.Data;
using MicrShopping.OrderApi.Models;
using MicrShopping.OrderApi.Services;
using Moq;
using Xunit;
using System.Linq;
using MicrShopping.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;

namespace MicrShopping.OrderApi.UnitTest
{
    public class ShoppingCartTest
    {
        public Mock<ProductService> productService;
        public Mock<IMapper> mapper;
        public Mock<IUserManage> userManage;
        public DbContextOptions<OrderDbContext> options;

        public ShoppingCartTest()
        {
            options = new DbContextOptionsBuilder<OrderDbContext>().UseInMemoryDatabase("testdb").Options;
            // 这里DbContext采用内存数据库，用Mock模拟会有很多问题，如果是有接口的IRepository也推荐用Mock模拟
            using (var dbContext = new OrderDbContext(options))
            {
                dbContext.AddRange(ShoppingCartList());
                dbContext.SaveChanges();
            }

            // Moq创建模拟类型的实现。如果类型是接口，则它将创建实现该接口的类。
            // 如果类型是类，则它将创建一个继承的类，并且该继承的类的成员称为基类。但是为了做到这一点，它必须覆盖成员。
            // 如果一个类的成员不能被覆盖（它们不是虚拟的，抽象的），则Moq不能覆盖它们以添加自己的行为。
            userManage = new Mock<IUserManage>();
            mapper = new Mock<IMapper>();
            productService = new Mock<ProductService>(null, null);
        }

        [Fact]
        public async Task AddShoppingCartTest1()
        {
            //在对一个方法的单元测试中，我们应该关注的是这个方法内部的逻辑测试，
            //而这个方法内部的外部依赖，则不在这个单元测试关注的范围内,所以我们只需要模拟外部依赖和外部依赖方法执行结果

            int userId = 1;
            int productId = 1;
            AddShoppingCartRequest request = new AddShoppingCartRequest()
            {
                ProductId = productId,
                Number = 1
            };
            var orderDbContext = new OrderDbContext(options);
            ShoppingCartController shoppingCartController = new ShoppingCartController(orderDbContext, mapper.Object, productService.Object, userManage.Object);
            userManage.Setup(_userManage => _userManage.GetUserId(shoppingCartController.User)).Returns(userId);

            await shoppingCartController.AddShoppingCart(request);

            int number = orderDbContext.ShoppingCart.FirstOrDefault(a => a.ProductId == productId && a.UserId == userId).Number;
            //断言  产品数量是否一致
            Assert.Equal(2, number);
        }

        [Fact]
        public async Task AddShoppingCartTest2()
        {
            int userId = 1;
            int productId = 2;//修改产品ID
            AddShoppingCartRequest request = new AddShoppingCartRequest()
            {
                ProductId = productId,
                Number = 1
            };
            var orderDbContext = new OrderDbContext(options);
            ShoppingCartController shoppingCartController = new ShoppingCartController(orderDbContext, mapper.Object, productService.Object, userManage.Object);
            userManage.Setup(_userManage => _userManage.GetUserId(shoppingCartController.User)).Returns(userId);

            await shoppingCartController.AddShoppingCart(request);
            int number = orderDbContext.ShoppingCart.FirstOrDefault(a => a.ProductId == productId && a.UserId == userId).Number;
            //断言  产品数量是否一致
            Assert.Equal(1, number);
        }

        public List<ShoppingCart> ShoppingCartList()
        {
            return new List<ShoppingCart>() {
                new ShoppingCart(){Id=1,Number=1,ProductId=1,UserId=1 }
            };
        }
    }
}