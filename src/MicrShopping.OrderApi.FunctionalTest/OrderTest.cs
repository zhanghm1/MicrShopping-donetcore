using System;
using System.Threading.Tasks;
using MicrShopping.OrderApi.Models;
using MicrShopping.OrderApi.FunctionalTest.Base;
using Xunit;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using MicrShopping.Infrastructure.Common;

namespace MicrShopping.OrderApi.FunctionalTest
{
    public class OrderTest : OrderTestProgram
    {
        [Fact]
        public async Task OrderCreate()
        {
            using (var server = CreateServer())
            {
                CreateOrderForShopingRequest request = new CreateOrderForShopingRequest()
                {
                    Address = "",
                    Phone = "",
                    ShopingIds = new List<int>() { 1 }
                };

                string url = OrderUrlPath.Create();

                HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(request));
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var response = await server.CreateClient().PostAsync(url, httpContent);
                var str = await response.Content.ReadAsStringAsync();
                response.EnsureSuccessStatusCode();
            }
        }

        [Fact]
        public async Task OrderList()
        {
            using (var server = CreateServer())
            {
                RequestPageBase request = new RequestPageBase()
                {
                    PageIndex = 1,
                    PageSize = 10
                };

                string url = OrderUrlPath.List(request);

                var response = await server.CreateClient().GetAsync(url);
                var str = await response.Content.ReadAsStringAsync();
                response.EnsureSuccessStatusCode();
            }
        }
    }
}