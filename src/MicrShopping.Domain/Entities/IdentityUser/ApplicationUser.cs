using Microsoft.AspNetCore.Identity;
using System;

namespace MicrShopping.Domain
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser<int>
    {
        public string NickName { get; set; }
        public int Sex { get; set; }
        public DateTime? Birthday { get; set; }


    }
}
