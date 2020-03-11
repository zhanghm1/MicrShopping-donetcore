using MicrShopping.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicrShopping.Domain.Entities.Products
{
    public class ProductClass : EntityBase
    {
        public string Code { get; set; }
        public string Name{get;set; }
        public string Description { get; set; }
    }
}
