using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MicrShopping.Infrastructure.Common;
using MicrShopping.Infrastructure.EFCore;
using MicrShopping.UserManageApi.Models;

namespace MicrShopping.UserManageApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;
        private ApplicationDbContext _dbContext;
        public UserController(ILogger<UserController> logger, ApplicationDbContext dbContext, IMapper mapper)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "Get")]
        [Authorize]
        public async Task<UserDetailResponse> Get(int id)
        {
            UserDetailResponse resp = new UserDetailResponse();

            var user = _dbContext.Users.Where(a => a.Id == id).FirstOrDefault();
            //user.NormalizedUserName
            resp = _mapper.Map<UserDetailResponse>(user);
            return resp;
        }
        [HttpGet]
        [Route("Identity")]
        [Authorize]
        public UserDetailResponse Identity()
        {
            UserDetailResponse resp = new UserDetailResponse();

            var user = _dbContext.Users.Where(a => a.Id == 1).FirstOrDefault();
            //user.NormalizedUserName
            resp = _mapper.Map<UserDetailResponse>(user);
            return resp;

        }
        [HttpGet]
        [Route("exception")]
        public IActionResult exception()
        {
            throw new ApiExceptionBase("ccccc","错误的");

        }
        [HttpGet]
        [Route("GetHtml")]
        public async Task<HttpResponseMessage> GetHtml()
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage resp = await httpClient.GetAsync("https://www.baidu.com/");

            return resp;

        }
    }
}
