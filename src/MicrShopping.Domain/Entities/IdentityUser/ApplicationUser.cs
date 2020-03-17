using Microsoft.AspNetCore.Identity;
using MicrShopping.Infrastructure.Common;
using System;

namespace MicrShopping.Domain
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser<int>, IEntityBase
    {
        public string NickName { get; set; }
        public int Sex { get; set; }
        public DateTime? Birthday { get; set; }


    }
}
