using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicrShopping.UserManageApi.Models
{
    public class UserDetailResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
        public string NormalizedUserName { get; set; }
        
        public int Sex { get; set; }
    }
}
