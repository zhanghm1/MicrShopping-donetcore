using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicrShopping.PayApi.Models
{
    public class PayRecoredRequest
    {
        public int UserId { get; set; }
        public string OrderNo { get; set; }
        public decimal Price { get; set; }
    }
}
