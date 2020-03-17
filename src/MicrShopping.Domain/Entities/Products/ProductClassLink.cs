
using MicrShopping.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicrShopping.Domain.Entities.Products
{
    /// <summary>
    ///   Link标识关联关系表
    /// </summary>
    public class ProductClassLink : EntityBase
    {
        public int ProductId { get; set; }
        public int ProductClassId { get; set; }
    }
}
