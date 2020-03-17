using Microsoft.AspNetCore.Identity;
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
            string ConnectionString = configuration["IdentityConnectionStrings"];
            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(ConnectionString));

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            return services;
        }

    }
}
