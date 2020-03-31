using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
using MicrShopping.Infrastructure.Common.ApiFilters;
using MicrShopping.OrderApi.Data;
using MicrShopping.PayApi.Data;
using RabbitMQ.Client;

namespace MicrShopping.PayApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment env { get; }
        public Startup(IConfiguration configuration, IWebHostEnvironment _env)
        {
            env = _env;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAutoMapper(typeof(Startup).Assembly);
            services.AddControllersWithViews(options => {
                options.Filters.Add<ApiExceptionFilter>();
                options.Filters.Add<ApiResultFilter>();
            }).AddWebApiConventions();//处理返回HttpResponseMessage

            services.AddScoped<PayDbContextSeed>();

            string Host = Configuration["ConnectionStrings:Host"];
            string Port = Configuration["ConnectionStrings:Port"];
            string Database = Configuration["ConnectionStrings:Database"];
            string Password = Configuration["ConnectionStrings:Password"];
            string UserID = Configuration["ConnectionStrings:UserID"];


            string ConnectionStrings = $"Host={Host};Port={Port};Database={Database};User ID={UserID};Password={Password};";
            services.AddDbContext<PayDbContext>(options =>
                   options.UseNpgsql(ConnectionStrings)
                   );

            string RabbitMQHost = Configuration["RabbitMQ:Host"];
            string RabbitMQPassword = Configuration["RabbitMQ:Password"];
            string RabbitMQUserName = Configuration["RabbitMQ:UserName"];
            string RabbitMQPort = Configuration["RabbitMQ:Port"];

            services.AddCap(x =>
            {
                x.UseEntityFramework<PayDbContext>();

                x.UseRabbitMQ(options =>
                {

                    options.HostName = RabbitMQHost;
                    options.Password = RabbitMQPassword;
                    options.UserName = RabbitMQUserName;
                    // docker内部访问使用默认端口就可以
                    //options.Port = Convert.ToInt32(RabbitMQPort);
                });
                //x.UseInMemoryStorage();
                //x.UseInMemoryMessageQueue();



                x.UseDashboard();
            });

            

            string IdentityUrl = Configuration["IdentityUrl"];// "http://192.168.0.189:5008";
            services.AddAuthentication("Bearer")
                 .AddJwtBearer("Bearer", options =>
                 {
                     options.Authority = IdentityUrl;
                     options.RequireHttpsMetadata = false;
                     options.Audience = "payapi";
                 });

            services.AddMvc(options => {
                options.Filters.Add<ApiExceptionFilter>();
                options.Filters.Add<ApiResultFilter>();
            });

            if (env.IsDevelopment())
            {
                services.AddConsulConfig(Configuration);
            }
            services.AddCors(options =>
            {
                // this defines a CORS policy called "default"
                options.AddPolicy("default", policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseConsul(Configuration);
            }
            else
            {
                //app.UseHttpsRedirection();
            }
            

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
