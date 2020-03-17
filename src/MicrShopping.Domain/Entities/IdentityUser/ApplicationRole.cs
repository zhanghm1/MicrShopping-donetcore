using Microsoft.AspNetCore.Identity;
using MicrShopping.Infrastructure.Common;

namespace MicrShopping.Domain
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationRole : IdentityRole<int>, IEntityBase
    {
    }
}
