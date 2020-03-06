using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MicrShopping.Domain.Extensions;
using MicrShopping.OrderApi.Data;
using MicrShopping.PayApi.Data;
using RabbitMQ.Client;

namespace MicrShopping.PayApi
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

            services.AddScoped<PayDbContextSeed>();

            string PayConnectionStrings = Configuration["PayConnectionStrings"];
            services.AddDbContext<PayDbContext>(options =>
                   options.UseMySql(PayConnectionStrings)
                   );

            string RabbitMQHost = Configuration["RabbitMQHost"];
            string RabbitMQPassword = Configuration["RabbitMQPassword"];
            string RabbitMQUserName = Configuration["RabbitMQUserName"];
            string RabbitMQPort = Configuration["RabbitMQPort"];

            services.AddCap(x =>
            {
                x.UseEntityFramework<PayDbContext>();

                x.UseRabbitMQ(options =>
                {

                    options.HostName = RabbitMQHost;
                    options.Password = RabbitMQPassword;
                    options.UserName = RabbitMQUserName;
                    // docker内部访问使用默认端口就可以
                    options.Port = Convert.ToInt32(RabbitMQPort);
                });
                x.UseDashboard();
            });

            services.AddConsulConfig(Configuration);
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
            app.UseConsul(Configuration);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
