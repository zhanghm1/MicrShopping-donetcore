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
using Microsoft.OpenApi.Models;
using MicrShopping.Domain.Extensions;
using MicrShopping.Infrastructure.Common.ApiFilters;
using MicrShopping.ProductApi.Data;

namespace MicrShopping.ProductApi
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

            services.AddScoped<ProductDbContextSeed>();

            string ProductConnectionStrings = Configuration["ProductConnectionStrings"];
            services.AddDbContext<ProductDbContext>(options =>
                   options.UseNpgsql(ProductConnectionStrings)
                   );

            string RabbitMQHost = Configuration["RabbitMQHost"];
            string RabbitMQPassword = Configuration["RabbitMQPassword"];
            string RabbitMQUserName = Configuration["RabbitMQUserName"];
            string RabbitMQPort = Configuration["RabbitMQPort"];

            services.AddCap(x =>
            {
                x.UseEntityFramework<ProductDbContext>();
                x.UseRabbitMQ(options =>
                {

                    options.HostName = RabbitMQHost;
                    options.Password = RabbitMQPassword;
                    options.UserName = RabbitMQUserName;
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
                     options.Audience = "productapi";
                 });
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

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            //});
            if (env.IsDevelopment())
            {
                services.AddConsulConfig(Configuration);
            }
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
            app.UseCors("default");


            app.UseRouting();

            //app.UseSwagger();
            // 生成自己的SwaggerUI  这里已经因为在ApiGateway项目集成多个服务的SwaggerUI，就注释了；只需要生成/swagger/v1/swagger.json文件让ApiGateway能获取到
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            //});


            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
