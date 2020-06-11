using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicrShopping.OrderApi.Models
{
    public class ShoppingCartListItemResponse
    {
        public int ShoppingCartId { get; set; }
        public int Number { get; set; }
        public int ProductId { get; set; }
        /// <summary>
        /// 唯一编号
        /// </summary>
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// 真实价格
        /// </summary>
        public decimal? RealPrice { get; set; }
        /// <summary>
        /// 原价
        /// </summary>
        public decimal? FormerPrice { get; set; }
    }
}
