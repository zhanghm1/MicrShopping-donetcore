using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicrShopping.OrderApi.Models
{
    public class AddShoppingCartRequest
    {
        public int ProductId { get; set; }
        public int Number { get; set; }
    }
}
