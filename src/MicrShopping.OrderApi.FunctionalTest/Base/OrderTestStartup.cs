using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicrShopping.OrderApi.Data;
using Savorboard.CAP.InMemoryMessageQueue;

namespace MicrShopping.OrderApi.FunctionalTest.Base
{
    public class OrderTestStartup : OrderApi.Startup
    {
        public OrderTestStartup(IConfiguration Configuration, IWebHostEnvironment env) : base(Configuration, env)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            // Added to avoid the Authorize data annotation in test environment.
            // Property "SuppressCheckForUnhandledSecurityMetadata" in appsettings.json
            services.Configure<RouteOptions>(Configuration);
            base.ConfigureServices(services);
            //不加会404  官方的eShopOnContainers项目也加了   Added for functional tests
            services.AddControllers().AddApplicationPart(typeof(OrderApi.Startup).Assembly);
        }

        protected override void AddDbContext(IServiceCollection services)
        {
            services.AddDbContext<OrderDbContext>(options =>
                   options.UseInMemoryDatabase("OrderDb")
                   );
            services.AddScoped<OrderDbContextSeed>();
        }

        protected override void ConfigureAuth(IApplicationBuilder app)
        {
            app.UseMiddleware<AutoAuthorizeMiddleware>();
            //app.UseAuthentication();
            app.UseAuthorization();
        }

        protected override void AddConsulConfig(IServiceCollection services)
        {
            //services.AddConsulConfig(Configuration);
        }

        protected override void UseConsul(IApplicationBuilder app)
        {
            //app.UseConsul(Configuration);
        }

        protected override void AddCap(IServiceCollection services)
        {
            services.AddCap(x =>
            {
                x.UseInMemoryStorage();
                x.UseInMemoryMessageQueue();
                //x.UseDashboard();
            });
        }
    }
}