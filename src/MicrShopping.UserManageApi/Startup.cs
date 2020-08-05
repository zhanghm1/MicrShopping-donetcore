using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MicrShopping.Domain;
using MicrShopping.Domain.Extensions;
using MicrShopping.Infrastructure.Common.ApiFilters;
using MicrShopping.Infrastructure.EFCore;
using MicrShopping.UserApi.GrpcService;

namespace MicrShopping.UserManageApi
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
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add<ApiExceptionFilter>();
                options.Filters.Add<ApiResultFilter>();
            }).AddWebApiConventions();//处理返回HttpResponseMessage

            services.AddGrpc();

            string Host = Configuration["ConnectionStrings:Host"];
            string Port = Configuration["ConnectionStrings:Port"];
            string Database = Configuration["ConnectionStrings:Database"];
            string Password = Configuration["ConnectionStrings:Password"];
            string UserID = Configuration["ConnectionStrings:UserID"];

            string ConnectionStrings = $"Host={Host};Port={Port};Database={Database};User ID={UserID};Password={Password};";
            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(ConnectionStrings));

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IUserManage, UserManage>();

            string IdentityUrl = Configuration["IdentityUrl"];// "http://192.168.0.189:5008";

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
                    options.ApiName = "usermanageapi";
                    options.RequireHttpsMetadata = false;
                });
            // 原生写法
            //.AddJwtBearer("Bearer", options =>
            // {
            //     options.Authority = IdentityUrl;
            //     options.RequireHttpsMetadata = false;
            //     options.Audience = "usermanageapi";
            // });

            string RabbitMQHost = Configuration["RabbitMQ:Host"];
            string RabbitMQPassword = Configuration["RabbitMQ:Password"];
            string RabbitMQUserName = Configuration["RabbitMQ:UserName"];
            string RabbitMQPort = Configuration["RabbitMQ:Port"];

            services.AddCap(x =>
            {
                x.UseEntityFramework<ApplicationDbContext>();
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
                // https 访问的ip地址会提示不安全,时候不要使用这个自动跳转，
                // nginx 会重定向
                //app.UseHttpsRedirection();
            }
            app.UseCors("default");

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<UserGrpcServcie>();

                endpoints.MapControllers();
            });
        }
    }
}