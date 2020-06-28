using MicrShopping.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicrShopping.Domain.Entities.Pays
{
    public class PayRecored: EntityBase
    {
        public int UserId { get; set; }
        public string OrderNo { get; set; }
        public decimal Price { get; set; }
        public PayStatus Status { get; set; }

    }

    public enum PayStatus
    {
        /// <summary>
        /// 待确认
        /// </summary>
        WaitConfirm,
        /// <summary>
        /// 完成
        /// </summary>
        Complete,
        /// <summary>
        /// 退款
        /// </summary>
        Refund,
    }
}
