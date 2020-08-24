using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MicrShopping.Domain.Extensions;
using MicrShopping.OrderApi.Data;

namespace MicrShopping.OrderApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Console.WriteLine(string.Join('-', args));
                var host = CreateHostBuilder(args).Build();

                host.MigrateDbContext<OrderDbContext, OrderDbContextSeed>();

                host.Run();
            }
            catch (Exception ex)
            {
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)

            .ConfigureAppConfiguration(config =>
            {
                config.AddEnvironmentVariables();
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}