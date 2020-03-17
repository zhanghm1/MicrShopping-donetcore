using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [Route("ProductList")]
        public PageBase<ProductListResponse> GetProductList([FromQuery]ProductListRequest request)
        {
            
            var list = _productDbContext.Product.OrderBy(a => a.Id).ToList();
            PageBase<ProductListResponse> resp = new PageBase<ProductListResponse>();
            resp.List = _mapper.Map<List<ProductListResponse>>(list.ToList());

            return resp;
        }

        [HttpGet]
        [Route("ProductClassList")]
        public PageBase<ProductClassResponse> GetProductClassList()
        {
            
            var list = _productDbContext.ProductClass.OrderBy(a => a.Id).ToList();
            PageBase<ProductClassResponse> resp = new PageBase<ProductClassResponse>();
            resp.List = _mapper.Map<List<ProductClassResponse>>(list.ToList());

            return resp;

        }
        [HttpGet]
        [Route("ProductDetail/{id}")]
        public ProductDetailResponse GetProductDetail(int id)
        {
            ProductDetailResponse resp = new ProductDetailResponse();

            var product = _productDbContext.Product.FirstOrDefault(a=>a.Id==id);

            var linq = from pc in _productDbContext.ProductClass
                       join pcl in _productDbContext.ProductClassLink on pc.Id equals pcl.ProductClassId
                       where pcl.ProductId == id
                       select new ProductClassResponse() {
                       Id=pc.Id,
                       Code=pc.Code,
                       Description=pc.Description,
                       Name=pc.Name
                       };
            resp = _mapper.Map<ProductDetailResponse>(product);
            resp.ProductClass= _mapper.Map<List<ProductClassResponse>>(linq.ToList());
            return resp;
        }

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
