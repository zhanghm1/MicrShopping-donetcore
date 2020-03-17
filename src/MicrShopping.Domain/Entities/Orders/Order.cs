using MicrShopping.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicrShopping.Domain.Entities.Orders
{
    public class Order: EntityBase
    {
        public string Code { get; set; }
        public decimal TotalPrice { get; set; }
        public string Address { get; set; }
        public OrderStatus Status { get; set; }
        /// <summary>
        /// 快递单号
        /// </summary>
        public string ExpressNumber { get; set; }
        public DateTime? PayTime { get; set; }
        public DateTime? SendTime { get; set; }
        public DateTime? ReceiptTime { get; set; }
        public DateTime? CommentTime { get; set; }
        public DateTime? AfterSaleTime { get; set; }

    }

    public enum OrderStatus
    { 
        WaitPay,                                                         
        WaitSend,
        WaitReceipt,
        WaitComment,
        AfterSale

    }
}
