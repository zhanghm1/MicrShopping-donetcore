using MicrShopping.Infrastructure.Common;
using MicrShopping.OrderApi.Models;
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


        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<ProductListResponse>> GetProductListByIds(string ids)
        {
            var httpclient = _httpClientFactory.CreateClient();
            string ProductUrl = "http://micrshopping.productapi/Product/ListByIds?ids=" + ids;
            var reslut = await httpclient.GetAsync(ProductUrl);
            if (reslut.IsSuccessStatusCode)
            {
                string reslutStr = await reslut.Content.ReadAsStringAsync();
                ResponseBase<List<ProductListResponse>> products1 = JsonConvert.DeserializeObject<ResponseBase<List<ProductListResponse>>>(reslutStr, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()

                });
                return products1.Data;
            }
            return null;
        }
    }
}
