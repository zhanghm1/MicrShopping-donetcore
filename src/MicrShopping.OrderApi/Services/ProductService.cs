//using AutoMapper.Configuration;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using MicrShopping.Infrastructure.Common;
using MicrShopping.OrderApi.Models;
using MicrShopping.protos;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MicrShopping.OrderApi.Services
{
    public class ProductService
    {
        private IHttpClientFactory _httpClientFactory;
        private IConfiguration _configuration;
        private string ProductGrpcUrl;
        private string ProductUrl;

        public ProductService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            ProductGrpcUrl = _configuration.GetValue<string>("ProductGrpcUrl");
            ProductUrl = _configuration.GetValue<string>("ProductUrl");
        }

        public async Task<List<Models.ProductListResponse>> GetProductListByIds(string ids)
        {
            var httpclient = _httpClientFactory.CreateClient();
            string url = ProductUrl+"/Product/ListByIds?ids=" + ids;
            var reslut = await httpclient.GetAsync(url);
            if (reslut.IsSuccessStatusCode)
            {
                string reslutStr = await reslut.Content.ReadAsStringAsync();
                ResponseBase<List<Models.ProductListResponse>> products1 = JsonConvert.DeserializeObject<ResponseBase<List<Models.ProductListResponse>>>(reslutStr, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()

                });
                return products1.Data;
            }
            return null;
        }

        public async Task<List<Models.ProductListResponse>> GetProductListByIdsGrpc(string ids)
        {
            return await GrpcServiceCaller.CallService(ProductGrpcUrl, async channel =>
            {
                var client = new ProductGrpcService.ProductGrpcServiceClient(channel);
                var reply = await client.ProductListByIdsAsync(new ProductListRequest { Ids = ids });

                return reply.Data.Select(item => new Models.ProductListResponse()
                {
                    Code = item.Code,
                    Description = item.Description,
                    FormerPrice = (decimal)item.FormerPrice,
                    Id = item.Id,
                    ImageUrl = item.ImageUrl,
                    Name = item.Name,
                    NowCount = item.NowCount,
                    RealPrice = (decimal)item.RealPrice
                }).ToList();
            });
        }

    }
}
