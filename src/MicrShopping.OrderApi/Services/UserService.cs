//using AutoMapper.Configuration;
using Grpc.Core;
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
    public class UserService
    {
        private IHttpClientFactory _httpClientFactory;
        private IConfiguration _configuration;
        private string UserGrpcUrl;

        public UserService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            UserGrpcUrl = _configuration.GetValue<string>("UserGrpcUrl");
        }

        public async Task<Models.UserListItemResponse> GetUserInfoByIdGrpc(int id,string token)
        {
            return await GrpcServiceCaller.CallService(UserGrpcUrl, async channel =>
            {
                var headers = new Metadata { { "Authorization", $"Bearer {token}" } };
                var client = new UserGrpcService.UserGrpcServiceClient(channel);
                var reply = await client.UserInfoByIdAsync(new  UserInfoRequest { Id = id }, headers);

                return new Models.UserListItemResponse()
                {
                    Id = reply.Id,
                    NickName = reply.NickName,
                    UserName = reply.UserName,

                };
            });
        }

    }
}
