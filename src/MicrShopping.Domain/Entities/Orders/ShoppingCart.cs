using MicrShopping.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicrShopping.Domain.Entities.Orders
{
    public class ShoppingCart : EntityBase
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Number { get; set; }
    }
}