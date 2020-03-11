using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicrShopping.ProductApi.Models
{
    public class ProductListResponse
    {
        public int Id { get; set; }
        /// <summary>
        /// 唯一编号
        /// </summary>
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// 真实价格
        /// </summary>
        public decimal RealPrice { get; set; }
        /// <summary>
        /// 原价
        /// </summary>
        public decimal FormerPrice { get; set; }
        /// <summary>
        /// 当前库存
        /// </summary>
        public int NowCount { get; set; }
        /// <summary>
        /// 累计销售
        /// </summary>
        public int SellCount { get; set; }

        public List<ProductClassResponse> ProductClass { get; set; }
    }
}
