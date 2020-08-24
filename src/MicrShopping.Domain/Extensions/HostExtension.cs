using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MicrShopping.Infrastructure.Common.BaseModels;

namespace MicrShopping.Domain.Extensions
{
    public static class HostExtension
    {
        public static void MigrateDbContext<TContext>(this IHost host)
    where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<TContext>();
                context.Database.Migrate();
            }
        }

        public static void MigrateDbContext<TContext, TDbSeed>(this IHost host)
            where TContext : DbContext
            where TDbSeed : IDbContextSeed
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<TContext>();
                context.Database.Migrate();

                var seedData = services.GetRequiredService<TDbSeed>();
                seedData.Init();
            }
        }
    }
}