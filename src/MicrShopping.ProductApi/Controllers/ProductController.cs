using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MicrShopping.Domain.Base;
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
        public ResponseBase<PageBase<ProductListResponse>> GetProductList([FromQuery]ProductListRequest request)
        {
            ResponseBase<PageBase<ProductListResponse>> resp = new ResponseBase<PageBase<ProductListResponse>>();

            var list = _productDbContext.Product.OrderBy(a => a.Id).ToList();
            PageBase<ProductListResponse> page = new PageBase<ProductListResponse>();
            page.List = _mapper.Map<List<ProductListResponse>>(list.ToList());

            resp.Data = page;
            return resp;
        }

        [HttpGet]
        [Route("ProductClassList")]
        public ResponseBase<PageBase<ProductClassResponse>> GetProductClassList()
        {
            ResponseBase<PageBase<ProductClassResponse>> resp = new ResponseBase<PageBase<ProductClassResponse>>();

            var list = _productDbContext.ProductClass.OrderBy(a => a.Id).ToList();
            PageBase<ProductClassResponse> page = new PageBase<ProductClassResponse>();
            page.List = _mapper.Map<List<ProductClassResponse>>(list.ToList());

            resp.Data = page;
            return resp;

        }
        [HttpGet]
        [Route("ProductDetail/{id}")]
        public ResponseBase<ProductDetailResponse> GetProductDetail(int id)
        {
            ResponseBase<ProductDetailResponse> resp = new ResponseBase<ProductDetailResponse>();

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
            resp.Data = _mapper.Map<ProductDetailResponse>(product);
            resp.Data.ProductClass= _mapper.Map<List<ProductClassResponse>>(linq.ToList());
            return resp;
        }

        [NonAction]
        [CapSubscribe(CapStatic.ReduceProductCount)]
        public async Task ReduceProductCount(List<ReduceProductModel> list)
        {
            Console.WriteLine("CapStatic.ReduceProductCount:" + JsonConvert.SerializeObject(list));
            foreach (var item in list)
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
    }
}
