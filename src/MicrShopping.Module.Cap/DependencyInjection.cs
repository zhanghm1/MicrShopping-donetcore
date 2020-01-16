using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MicrShopping.Module.Cap
{
    public static class DependencyInjection
    {
        public static void AddMicrShoppingCap(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddCap(x =>
            {
                x.UseSqlServer(Configuration.GetValue<string>("CapConnectionStrings"));

                x.UseRabbitMQ(options=> {
                    options.HostName = "127.0.0.1";
                    options.Password = "guest";
                    options.UserName = "guest";
                });
                x.UseDashboard();
            });
        }
    }
}
