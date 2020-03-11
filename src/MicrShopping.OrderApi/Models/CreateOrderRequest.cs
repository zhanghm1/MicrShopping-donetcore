using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicrShopping.OrderApi.Models
{
    public class CreateOrderRequest
    {
        public List<CreateOrderItemRequest> Data { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
    public class CreateOrderItemRequest
    {
        public int ProductId { get; set; }
        public int Number { get; set; }
    }
}
