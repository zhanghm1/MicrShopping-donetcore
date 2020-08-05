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
            // ����DbContext�����ڴ����ݿ⣬��Mockģ����кܶ����⣬������нӿڵ�IRepositoryҲ�Ƽ���Mockģ��
            using (var dbContext = new OrderDbContext(options))
            {
                dbContext.AddRange(ShoppingCartList());
                dbContext.SaveChanges();
            }

            // Moq����ģ�����͵�ʵ�֡���������ǽӿڣ�����������ʵ�ָýӿڵ��ࡣ
            // ����������࣬����������һ���̳е��࣬���Ҹü̳е���ĳ�Ա��Ϊ���ࡣ����Ϊ��������һ�㣬�����븲�ǳ�Ա��
            // ���һ����ĳ�Ա���ܱ����ǣ����ǲ�������ģ�����ģ�����Moq���ܸ�������������Լ�����Ϊ��
            userManage = new Mock<IUserManage>();
            mapper = new Mock<IMapper>();
            productService = new Mock<ProductService>(null, null);
        }

        [Fact]
        public async Task AddShoppingCartTest1()
        {
            //�ڶ�һ�������ĵ�Ԫ�����У�����Ӧ�ù�ע������������ڲ����߼����ԣ�
            //����������ڲ����ⲿ���������������Ԫ���Թ�ע�ķ�Χ��,��������ֻ��Ҫģ���ⲿ�������ⲿ��������ִ�н��

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
            //����  ��Ʒ�����Ƿ�һ��
            Assert.Equal(2, number);
        }

        [Fact]
        public async Task AddShoppingCartTest2()
        {
            int userId = 1;
            int productId = 2;//�޸Ĳ�ƷID
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
            //����  ��Ʒ�����Ƿ�һ��
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