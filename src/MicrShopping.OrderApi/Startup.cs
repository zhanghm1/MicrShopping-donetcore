using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MicrShopping.OrderApi.Data;

namespace MicrShopping.OrderApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddScoped<OrderDbContextSeed>();

            string OrderConnectionStrings = Configuration["OrderConnectionStrings"];
            services.AddDbContext<OrderDbContext>(options =>
                   options.UseSqlServer(OrderConnectionStrings)
                   );

            services.AddCap(x =>
            {
                x.UseEntityFramework<OrderDbContext>();

                x.UseRabbitMQ(options => {
                    options.HostName = Configuration["RabbitMQHost"];
                    options.Password = Configuration["RabbitMQPassword"];
                    options.UserName = Configuration["RabbitMQUserName"];
                    //options.Port = Convert.ToInt32(Configuration["RabbitMQPort"]);
                });
                x.UseDashboard();
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        
    }
}
