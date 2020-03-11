using MicrShopping.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicrShopping.Domain.Entities.Orders
{
    public class OrderItem: EntityBase
    {
        public string OrderCode { get; set; }
        public string Code { get; set; }
        public string ProducName { get; set; }
        public int ProductId { get; set; }
        /// <summary>
        /// 真实价格
        /// </summary>
        public decimal RealPrice { get; set; }
        /// <summary>
        /// 原价
        /// </summary>
        public decimal FormerPrice { get; set; }
        public int Number { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
