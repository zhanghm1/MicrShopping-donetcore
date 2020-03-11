using AutoMapper;
using MicrShopping.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicrShopping.ProductApi.Models.ModelMaps
{
    public class AutoMapperConfigs : Profile
    {
        public AutoMapperConfigs()
        {
            CreateMap<Product, ProductDetailResponse>();
            CreateMap<Product, ProductListResponse>();
            CreateMap<ProductClass, ProductClassResponse>();
        }
    }
}
