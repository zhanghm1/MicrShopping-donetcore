using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicrShopping.OrderApi.Data;

namespace MicrShopping.OrderApi.FunctionalTest.Base
{
    public class OrderTestProgram
    {
        public TestServer CreateServer()
        {
            var path = Assembly.GetAssembly(typeof(OrderTestProgram))
              .Location;

            var hostBuilder = new WebHostBuilder()
                .UseContentRoot(Path.GetDirectoryName(path))
                .ConfigureAppConfiguration((webContent, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false);
                    config.AddEnvironmentVariables();
                })
                .UseEnvironment("Development")
                .UseStartup<OrderTestStartup>();

            var testServer = new TestServer(hostBuilder);

            using (var scope = testServer.Host.Services.CreateScope())
            {
                var dbSeedData = scope.ServiceProvider.GetRequiredService<OrderDbContextSeed>();
                dbSeedData.Init();
            }

            return testServer;
        }
    }
}