using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicrShopping.OrderApi.Models
{
    public class UserListItemResponse
    {
        public int Id { get; set; }
        public string NickName { get; set; }
        public string UserName { get; set; }
    }
}
