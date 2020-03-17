using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MicrShopping.Domain.Base;
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
        public async Task<ResponseBase<UserDetailResponse>> Get(int id)
        {
            ResponseBase<UserDetailResponse> resp = new ResponseBase<UserDetailResponse>();

            var user = _dbContext.Users.Where(a => a.Id == id).FirstOrDefault();
            //user.NormalizedUserName
            resp.Data = _mapper.Map<UserDetailResponse>(user);
            return resp;
        }
        [HttpGet]
        [Route("Identity")]
        [Authorize]
        public IActionResult Identity()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });

        }
    }
}
