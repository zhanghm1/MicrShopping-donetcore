using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MicrShopping.Domain;
using MicrShopping.Domain.Extensions;
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
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            //services.AddScoped<OrderDbContextSeed>();

            services.AddIdentityEFCore(Configuration);

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

            services.AddConsulConfig(Configuration);

            string IdentityUrl = Configuration["IdentityUrl"];// "http://192.168.0.189:5008";

            services.AddAuthentication("Bearer")
                 .AddJwtBearer("Bearer", options =>
                 {
                     options.Authority = IdentityUrl;
                     options.RequireHttpsMetadata = false;
                     options.Audience = "usermanageapi";
                 });

            //����Authorize��policy,������Ӷ��
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

            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHttpsRedirection();
            }
            // https ���ʵĵ�ַ����ʾ����ȫ��ʱ��Ҫʹ������Զ���ת��
            //app.UseHttpsRedirection();
            app.UseCors("default");


            app.UseRouting();

            app.UseConsul(Configuration);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            // �����Լ���SwaggerUI  �����Ѿ���Ϊ��ApiGateway��Ŀ���ɶ�������SwaggerUI����ע���ˣ�ֻ��Ҫ����/swagger/v1/swagger.json�ļ���ApiGateway�ܻ�ȡ��
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
