using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MicrShopping.Domain;
using MicrShopping.Domain.Extensions;
using MicrShopping.Infrastructure.Common.ApiFilters;
using MicrShopping.OrderApi.Data;
using MicrShopping.OrderApi.Services;

namespace MicrShopping.OrderApi
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


            services.AddScoped<OrderDbContextSeed>();
            services.AddScoped<ProductService>();
            services.AddScoped<UserService>();

            string Host = Configuration["ConnectionStrings:Host"];
            string Port = Configuration["ConnectionStrings:Port"];
            string Database = Configuration["ConnectionStrings:Database"];
            string Password = Configuration["ConnectionStrings:Password"];
            string UserID = Configuration["ConnectionStrings:UserID"];


            string ConnectionStrings = $"Host={Host};Port={Port};Database={Database};User ID={UserID};Password={Password};";

            Console.WriteLine(ConnectionStrings);
            services.AddDbContext<OrderDbContext>(options =>
                   options.UseNpgsql(ConnectionStrings)
                   );
            services.AddScoped<OrderDbContext>();

            string RabbitMQHost = Configuration["RabbitMQ:Host"];
            string RabbitMQPassword = Configuration["RabbitMQ:Password"];
            string RabbitMQUserName = Configuration["RabbitMQ:UserName"];
            string RabbitMQPort = Configuration["RabbitMQ:Port"];

            services.AddCap(x =>
            {
                x.UseEntityFramework<OrderDbContext>();
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
            

            string IdentityUrl= Configuration["IdentityUrl"];

            //services.AddAuthentication("Bearer")
            //     .AddJwtBearer("Bearer", options =>
            //     {
            //         options.Authority = IdentityUrl;
            //         options.RequireHttpsMetadata = false;
            //         options.Audience = "orderapi";
            //     });
            //保持结构一致，要么多个服务都使用上面的结构，要么统一用下面的结构，不要分开用，分开会对 ClaimsPrincipal 解析不一致
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                options.DefaultForbidScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
            })
            .AddIdentityServerAuthentication(options =>
            {
                options.Authority = IdentityUrl;
                options.ApiName = "orderapi";
                options.RequireHttpsMetadata = false;
            });



            //设置Authorize的policy,可以添加多个
            services.AddAuthorization(options =>
            {
                options.AddPolicy("admin", policyAdmin =>
                {
                    policyAdmin.RequireClaim("role", "admin");
                });
                options.AddPolicy("user_admin", policyAdmin =>
                {
                    policyAdmin.RequireClaim("role", "user_admin");
                });
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


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

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
            // https 访问的地址会提示不安全的时候不要使用这个自动跳转，
            //app.UseHttpsRedirection();
            app.UseCors("default");

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();//生成/swagger/v1/swagger.json

            // 生成自己的SwaggerUI  这里已经因为在ApiGateway项目集成多个服务的SwaggerUI，就注释了；只需要生成/swagger/v1/swagger.json文件让ApiGateway能获取到
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

    }
}
