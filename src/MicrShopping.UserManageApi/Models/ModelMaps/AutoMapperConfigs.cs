using AutoMapper;
using MicrShopping.Domain;
using MicrShopping.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicrShopping.UserManageApi.Models.ModelMaps
{
    public class AutoMapperConfigs : Profile
    {
        public AutoMapperConfigs()
        {
            CreateMap<ApplicationUser, UserDetailResponse>();
        }
    }
}
