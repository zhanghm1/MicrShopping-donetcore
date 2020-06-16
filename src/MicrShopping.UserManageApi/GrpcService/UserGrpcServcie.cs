using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using MicrShopping.Infrastructure.EFCore;
using MicrShopping.protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicrShopping.UserApi.GrpcService
{
    public class UserGrpcServcie : UserGrpcService.UserGrpcServiceBase
    {
        private ApplicationDbContext _userDbContext;
        public UserGrpcServcie(ApplicationDbContext userDbContext)
        {
            _userDbContext = userDbContext;

        }
        [Authorize]
        public override Task<UserListResponse> UserListByIds(UserListRequest request, ServerCallContext context)
        {
            if (string.IsNullOrWhiteSpace(request.Ids))
            {
                return null;
            }
            int[] _ids = request.Ids.Split(',').Select(a => Convert.ToInt32(a)).ToArray();
            var list = _userDbContext.Users.Where(a => _ids.Contains(a.Id)).ToList();


            UserListResponse resp = new UserListResponse();
            list.ForEach(item =>
            {
                resp.Data.Add(new UserListItemResponse()
                {
                    Id=item.Id,
                    NickName=item.NickName??"",
                    UserName=item.UserName
                });
            });
            return Task.FromResult(resp);
        }
        [Authorize]
        public override Task<UserListItemResponse> UserInfoById(UserInfoRequest request, ServerCallContext context)
        {
            var user = _userDbContext.Users.Where(a => a.Id== request.Id).FirstOrDefault();
            if (user is null)
            {
                return null;
            }

            UserListItemResponse resp = new UserListItemResponse()
            {
                Id = user.Id,
                NickName = user.NickName ?? "",
                UserName = user.UserName
            };
            return Task.FromResult(resp);
        }
    }
}
