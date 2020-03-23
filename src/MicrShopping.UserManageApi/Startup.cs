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

namespace MicrShopping.UserManageApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services, IWebHostEnvironment env)
        {
            services.AddAutoMapper(typeof(Startup).Assembly);
            services.AddControllersWithViews(options => {
                options.Filters.Add<ApiExceptionFilter>();
                options.Filters.Add<ApiResultFilter>();
            }).AddWebApiConventions();//处理返回HttpResponseMessage


            string ConnectionString = Configuration["IdentityConnectionStrings"];
            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(ConnectionString));

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


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

            string RabbitMQHost = Configuration["RabbitMQHost"];
            string RabbitMQPassword = Configuration["RabbitMQPassword"];
            string RabbitMQUserName = Configuration["RabbitMQUserName"];
            string RabbitMQPort = Configuration["RabbitMQPort"];

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
                app.UseHttpsRedirection();
            }
            app.UseCors("default");

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
