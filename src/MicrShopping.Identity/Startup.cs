// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MicrShopping.Identity.Certificates;
using MicrShopping.Domain.Extensions;
using MicrShopping.Domain;
using MicrShopping.Infrastructure.EFCore;
using Microsoft.AspNetCore.HttpOverrides;

namespace MicrShopping.Identity
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
            services.Configure<IISServerOptions>(options =>
            {
                options.AutomaticAuthentication = false;
            });
            services.Configure<IISOptions>(options =>
            {
                options.ForwardClientCertificate = false;
            });

            services.AddControllersWithViews();

            string Host = Configuration["ConnectionStrings:Host"];
            string Port = Configuration["ConnectionStrings:Port"];
            string Database = Configuration["ConnectionStrings:Database"];
            string Password = Configuration["ConnectionStrings:Password"];
            string UserID = Configuration["ConnectionStrings:UserID"];

            string ConnectionStrings = $"Host={Host};Port={Port};Database={Database};User ID={UserID};Password={Password};";
            System.Console.WriteLine(ConnectionStrings);
            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(ConnectionStrings));

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
                .AddInMemoryIdentityResources(Config.Ids)
                .AddInMemoryApiResources(Config.Apis)
                .AddInMemoryClients(Config.Clients(Configuration))
                .AddAspNetIdentity<ApplicationUser>()
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator<ApplicationUser>>()
                .AddProfileService<ProfileService<ApplicationUser>>();

            // not recommended for production - you need to store your key material somewhere secure
            builder.AddSigningCredential(Certificate.Get());

            if (env.IsDevelopment())
            {
                services.AddConsulConfig(Configuration);
            }
            else
            {
                services.Configure<ForwardedHeadersOptions>(options =>
                {
                    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                });
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

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseConsul(Configuration);
            }
            else
            {
                // 当通过 HTTP 代理 HTTPS 请求时，原方案(HTTPS) 将丢失，并且必须在标头中转接。
                // 由于应用收到来自代理的请求，而不是 Internet 或公司网络上请求的真实源，因此原始客户端 IP 地址也必须在标头中转接。
                app.UseForwardedHeaders();

                ///// 也可以强制所有请求https

                //app.Use((context, next) =>
                //{
                //    System.Console.WriteLine("===================" + context.Request.Host.Host);
                //    context.Request.Scheme = "https";
                //    return next();
                //});
            }

            app.UseStaticFiles();

            app.UseCors("default");

            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}