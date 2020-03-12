using System;
using System.Collections.Generic;
using System.Text;

namespace MicrShopping.Infrastructure.Common.CapModels
{
    public class ReduceProductModel
    {
        public string OrderCode { get; set; }
        public List<ReduceProductItemModel> ProductItem { get; set; }
    }
    public class ReduceProductItemModel
    {
        public int Id { get; set; }
        public int Number { get; set; }
    }
}
