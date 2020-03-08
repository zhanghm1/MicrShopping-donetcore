
    using System;
using System.Collections.Generic;
using System.Linq;
    using Consul;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Hosting.Server.Features;
    using Microsoft.AspNetCore.Http.Features;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
namespace MicrShopping.Domain.Extensions
{
    public static class AppExtensions
    {
        public static IServiceCollection AddConsulConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
            {
                var address = configuration.GetSection("ConsulAddress").Value;
                consulConfig.Address = new Uri(address);

            }));
            return services;
        }

        public static IApplicationBuilder UseConsul(this IApplicationBuilder app, IConfiguration configuration)
        {
            var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
            var logger = app.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger("AppExtensions");
            var lifetime = app.ApplicationServices.GetRequiredService<IApplicationLifetime>();

            if (!(app.Properties["server.Features"] is FeatureCollection features)) return app;


            //- scheme=http
            //- ip=localhost
            //- port=25667
            //- ConsulAddress=http://localhost:8500
            //- ServerName=Shopping
            string ServerName = configuration.GetSection("ServerName").Value;
            string scheme = configuration.GetSection("scheme").Value;
            string ip = configuration.GetSection("ip").Value;
            string port = configuration.GetSection("port").Value;
            string serverAddress = $"{scheme}://{ip}:{port}";


            Console.WriteLine($"address={serverAddress}");

            var uri = new Uri(serverAddress);
            string Health = $"{uri.Scheme}://{uri.Host}:{uri.Port}/Test/Health"; //健康检查地址
            var registration = new AgentServiceRegistration()
            {
                ID = $"{ServerName}-{uri.Port}",
                Name = ServerName,
                Address = $"{uri.Host}",
                Port = uri.Port,
                Check =  new AgentServiceCheck()
                    {
                        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),   //服务启动多久后注册
                        Interval = TimeSpan.FromSeconds(10),                        //健康检查时间间隔，或者称为心跳间隔
                        HTTP = Health, //健康检查地址
                        Timeout = TimeSpan.FromSeconds(8),
                        
                        
                    }
                ,
                // Meta= new Dictionary<string, string>()

            };
            // registration.Meta.Add("test","11111");

            logger.LogInformation("Registering with Consul");

            consulClient.Agent.ServiceRegister(registration).Wait();

            

            lifetime.ApplicationStopping.Register(() =>
            {
                logger.LogInformation("Unregistering from Consul");
                consulClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
            });

            return app;
        }
    }
}