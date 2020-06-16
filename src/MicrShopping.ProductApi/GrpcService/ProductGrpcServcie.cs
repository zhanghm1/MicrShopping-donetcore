using Grpc.Core;
using MicrShopping.ProductApi.Data;
using MicrShopping.protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicrShopping.ProductApi.GrpcService
{
    public class ProductGrpcServcie: ProductGrpcService.ProductGrpcServiceBase
    {
        private ProductDbContext _productDbContext;
        public ProductGrpcServcie(ProductDbContext productDbContext)
        {
            _productDbContext = productDbContext;

        }

        public override Task<ProductListResponse> ProductListByIds(ProductListRequest request, ServerCallContext context)
        {
            if (string.IsNullOrWhiteSpace(request.Ids))
            {
                return null;
            }
            int[] _ids = request.Ids.Split(',').Select(a => Convert.ToInt32(a)).ToArray();
            var list = _productDbContext.Product.Where(a => _ids.Contains(a.Id)).ToList();


            ProductListResponse resp = new ProductListResponse();
            list.ForEach(item =>
            {
                resp.Data.Add(new ProductListItemResponse()
                {
                    Code= item.Code ?? "",
                    Description= item.Description??"",
                    FormerPrice= (double)item.FormerPrice,
                    Id= item.Id,
                    ImageUrl= item.ImageUrl ?? "",
                    Name= item.Name ?? "",
                    NowCount= item.NowCount,
                    RealPrice= (double)item.RealPrice
                });
            });
            return Task.FromResult(resp);
        }
    }
}
