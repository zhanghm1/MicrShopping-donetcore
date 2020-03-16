using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicrShopping.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicrShopping.Infrastructure.EFCore
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIdentityEFCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString("IdentityConnectionStrings")));

            return services;
        }

    }
}
