using MicrShopping.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MicrShopping.Domain.Entities.Products
{
    public class Product : EntityBase
    {
        /// <summary>
        /// 唯一编号
        /// </summary>
        public string Code { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 封面图
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// 详情的轮播图
        /// </summary>
        public string DetailUrl { get; set; }
        /// <summary>
        /// 详情的轮播图列表数据
        /// </summary>
        [NotMapped]
        public List<string> DetailUrlList
        {
            get
            {
                if (string.IsNullOrWhiteSpace(DetailUrl))
                {
                    return new List<string>();
                }
                return DetailUrl.Split(',').ToList();
            }
            set {
                DetailUrl = string.Join(',', value);
            }
        }
        /// <summary>
        /// 描述
        /// </summary>
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

    }
}
