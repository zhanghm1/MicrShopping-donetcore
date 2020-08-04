using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MicrShopping.Domain.Entities.Products;
using MicrShopping.Infrastructure.Common;
using MicrShopping.Infrastructure.Common.CapModels;
using MicrShopping.ProductApi.Data;
using MicrShopping.ProductApi.Models;
using Newtonsoft.Json;

namespace MicrShopping.ProductApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<ProductController> _logger;
        private ProductDbContext _productDbContext;

        public ProductController(ILogger<ProductController> logger, ProductDbContext productDbContext, IMapper mapper)
        {
            _logger = logger;
            _productDbContext = productDbContext;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("List")]
        public async Task<PageBase<ProductListResponse>> GetProductList([FromQuery] ProductListRequest request)
        {
            var query = _productDbContext.Product.Where(a => !a.IsDeleted);
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                query = query.Where(a => a.Name.Contains(request.Name));
            }
            var list = query.OrderBy(a => a.Id).Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).ToList();

            PageBase<ProductListResponse> resp = new PageBase<ProductListResponse>();
            resp.List = _mapper.Map<List<ProductListResponse>>(list.ToList());
            resp.PageIndex = request.PageIndex;
            resp.PageSize = request.PageSize;
            resp.CountTotal = await query.CountAsync();
            resp.PageTotal = (int)Math.Ceiling((double)resp.CountTotal / (double)resp.PageSize);
            return resp;
        }

        [HttpGet]
        [Route("ListByIds")]
        public List<ProductListResponse> GetProductList(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                return null;
            }
            int[] _ids = ids.Split(',').Select(a => Convert.ToInt32(a)).ToArray();
            var list = _productDbContext.Product.Where(a => _ids.Contains(a.Id)).ToList();

            return _mapper.Map<List<ProductListResponse>>(list.ToList());
        }

        [HttpGet]
        [Route("ClassList")]
        public PageBase<ProductClassResponse> GetProductClassList()
        {
            var list = _productDbContext.ProductClass.OrderBy(a => a.Id).ToList();
            PageBase<ProductClassResponse> resp = new PageBase<ProductClassResponse>();
            resp.List = _mapper.Map<List<ProductClassResponse>>(list.ToList());

            return resp;
        }

        [HttpGet]
        [Route("Detail/{id}")]
        public ProductDetailResponse GetProductDetail(int id)
        {
            ProductDetailResponse resp = new ProductDetailResponse();

            var product = _productDbContext.Product.FirstOrDefault(a => a.Id == id);

            var linq = from pc in _productDbContext.ProductClass
                       join pcl in _productDbContext.ProductClassLink on pc.Id equals pcl.ProductClassId
                       where pcl.ProductId == id
                       select new ProductClassResponse()
                       {
                           Id = pc.Id,
                           Code = pc.Code,
                           Description = pc.Description,
                           Name = pc.Name
                       };
            resp = _mapper.Map<ProductDetailResponse>(product);
            resp.ProductClass = _mapper.Map<List<ProductClassResponse>>(linq.ToList());
            return resp;
        }

        /// <summary>
        /// 创建订单时减少产品库存数量
        /// </summary>
        /// <param name="reduceProductModel"></param>
        /// <returns></returns>
        [NonAction]
        [CapSubscribe(CapStatic.ReduceProductCount)]
        public async Task ReduceProductCount(ReduceProductModel reduceProductModel)
        {
            Console.WriteLine("CapStatic.ReduceProductCount:" + JsonConvert.SerializeObject(reduceProductModel));
            foreach (var item in reduceProductModel.ProductItem)
            {
                var productInfo = _productDbContext.Product.Where(a => a.Id == item.Id).FirstOrDefault();
                if (productInfo.NowCount < item.Number)
                {
                    throw new Exception("数量不足");
                }
                productInfo.NowCount = productInfo.NowCount - item.Number;
            }

            _productDbContext.SaveChanges();
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